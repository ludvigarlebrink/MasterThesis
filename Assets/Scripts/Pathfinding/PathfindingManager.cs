using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PathfindingGrid))]
public class PathfindingManager : MonoBehaviour
{
    private PathfindingGrid m_Grid = null;

    public bool FindPath(Vector3 startWorldPoint, Vector3 endWorldPoint, ref List<Vector3> path, bool local)
    {
        PathfindingNode startNode = m_Grid.GetNodeFromWorldPoint(startWorldPoint);
        if (startNode == null)
        {
            return false;
        }

        PathfindingNode targetNode = m_Grid.GetNodeFromWorldPoint(endWorldPoint);
        if (targetNode == null)
        {
            return false;
        }

        if (startNode == targetNode)
        {
            return false;
        }

        // List of nodes for the open list.
        List<PathfindingNode> openList = new List<PathfindingNode>();

        // List of nodes for the closed list.
        HashSet<PathfindingNode> closedList = new HashSet<PathfindingNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            PathfindingNode currentNode = openList[0];

            for (int i = 1; i < openList.Count; ++i)
            {
                if (openList[i].fCost < currentNode.fCost || openList[i].fCost == currentNode.fCost && openList[i].hCost < currentNode.hCost)
                {
                    currentNode = openList[i];
                }
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode == targetNode)
            {
                path = GetFinalPath(startNode, targetNode, local);
                return true;
            }

            foreach (PathfindingNode neighborNode in m_Grid.GetNeighboringNodes(currentNode))
            {
                if (neighborNode.isObstacle || closedList.Contains(neighborNode))
                {
                    continue;
                }

                int moveCost = currentNode.gCost + GetManhattenDistance(currentNode, neighborNode);

                if (moveCost < neighborNode.gCost || !openList.Contains(neighborNode))
                {
                    neighborNode.gCost = moveCost;
                    neighborNode.hCost = GetManhattenDistance(neighborNode, targetNode);
                    neighborNode.parentNode = currentNode;

                    if (!openList.Contains(neighborNode))
                    {
                        openList.Add(neighborNode);
                    }
                }
            }
        }

        return false;
    }

    // Added by Chris to check if start and end positions are on the same node
    public PathfindingNode GetNode(Vector3 worldPoint)
    {
        return m_Grid.GetNodeFromWorldPoint(worldPoint);
    }

    private void Awake()
    {
        m_Grid = GetComponent<PathfindingGrid>();
    }

    private List<Vector3> GetFinalPath(PathfindingNode startNode, PathfindingNode endNode, bool local)
    {
        List<Vector3> cornerNodes = new List<Vector3>();
        PathfindingNode currentNode = endNode;

        int xD = 0;
        int yD = 0;

        while (currentNode != startNode)
        {
            if (xD != currentNode.gridX - currentNode.parentNode.gridX || yD != currentNode.gridZ - currentNode.parentNode.gridZ)
            {
                if (local)
                {
                    cornerNodes.Add(currentNode.position);
                }
                else
                {
                    cornerNodes.Add(transform.TransformPoint(currentNode.position));
                }
            }

            xD = currentNode.gridX - currentNode.parentNode.gridX;
            yD = currentNode.gridZ - currentNode.parentNode.gridZ;

            currentNode = currentNode.parentNode;
        }

        if (local)
        {
            cornerNodes.Add(startNode.position);
        }
        else
        {
            cornerNodes.Add(transform.TransformPoint(startNode.position));
        }

        cornerNodes.Reverse();
        return cornerNodes;
    }

    private int GetManhattenDistance(PathfindingNode nodeA, PathfindingNode nodeB)
    {
        int x = Mathf.Abs(nodeA.gridX - nodeB.gridX);
        int y = Mathf.Abs(nodeA.gridZ - nodeB.gridZ);

        int cost = x * 10 + y * 10;
        cost -= 6 * (x < y ? x : y);

        return cost;
    }
}
