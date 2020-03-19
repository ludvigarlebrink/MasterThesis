using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid : MonoBehaviour
{
    public LayerMask obstacleMask;
    public float gridWorldSizeX = 10;
    public float gridWorldSizeZ = 10;
    public float nodeRadius = 0.5f;
    public float distanceBetweenNodes = 2.0f;

    [SerializeField] private PathfindingNode[,] m_NodeArray = null;
    [SerializeField] private int m_GridSizeX = 0;
    [SerializeField] private int m_GridSizeZ = 0;

    public void GenerateGrid()
    {
        m_GridSizeX = Mathf.RoundToInt(gridWorldSizeX / distanceBetweenNodes);
        m_GridSizeZ = Mathf.RoundToInt(gridWorldSizeZ / distanceBetweenNodes);
        m_NodeArray = new PathfindingNode[m_GridSizeX, m_GridSizeZ];

        Vector3 bottomLeft = Vector3.zero;
        bottomLeft.x = (gridWorldSizeX / -2) + (distanceBetweenNodes / 2.0f);
        bottomLeft.z = (gridWorldSizeZ / -2) + (distanceBetweenNodes / 2.0f);

        for (int z = 0; z < m_GridSizeZ; ++z)
        {
            for (int x = 0; x < m_GridSizeX; ++x)
            {
                Vector3 position = bottomLeft + new Vector3(x * distanceBetweenNodes, 0.0f, z * distanceBetweenNodes);
                bool isObstacle = false;

                if (Physics.CheckSphere(transform.position + position, nodeRadius, obstacleMask))
                {
                    isObstacle = true;
                }

                m_NodeArray[x, z] = new PathfindingNode(isObstacle, position, x, z);
            }
        }
    }

    public PathfindingNode GetNodeFromWorldPoint(Vector3 worldPoint)
    {
        Vector3 offset = Vector3.zero;
        offset.x = (gridWorldSizeX / 2);
        offset.z = (gridWorldSizeZ / 2);

        Vector3 localPoint = (offset + transform.InverseTransformPoint(worldPoint)) / distanceBetweenNodes;

        int x = Mathf.FloorToInt(localPoint.x);
        int z = Mathf.FloorToInt(localPoint.z);

        if (x < 0 || x >= m_GridSizeX || z < 0 || z >= m_GridSizeZ)
        {
            return null;
        }

        return m_NodeArray[x, z];
    }

    public List<PathfindingNode> GetNeighboringNodes(PathfindingNode neighborNode)
    {
        List<PathfindingNode> neighborList = new List<PathfindingNode>();
        int checkX;
        int checkZ;

        checkX = neighborNode.gridX + 1;
        checkZ = neighborNode.gridZ;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        checkX = neighborNode.gridX - 1;
        checkZ = neighborNode.gridZ;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        checkX = neighborNode.gridX;
        checkZ = neighborNode.gridZ + 1;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        checkX = neighborNode.gridX;
        checkZ = neighborNode.gridZ - 1;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        checkX = neighborNode.gridX + 1;
        checkZ = neighborNode.gridZ + 1;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        checkX = neighborNode.gridX + 1;
        checkZ = neighborNode.gridZ - 1;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        checkX = neighborNode.gridX - 1;
        checkZ = neighborNode.gridZ + 1;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        checkX = neighborNode.gridX - 1;
        checkZ = neighborNode.gridZ - 1;
        if (checkX >= 0 && checkX < m_GridSizeX)
        {
            if (checkZ >= 0 && checkZ < m_GridSizeZ)
            {
                neighborList.Add(m_NodeArray[checkX, checkZ]);
            }
        }

        return neighborList;
    }

    private void Awake()
    {
        GenerateGrid();
    }

    private void OnDrawGizmos()
    {
        // Nothing to draw.
        if (m_NodeArray == null)
        {
            return;
        }

        Vector3 bottomLeft = Vector3.zero;
        bottomLeft.x = (gridWorldSizeX / -2) + distanceBetweenNodes / 2.0f;
        bottomLeft.z = (gridWorldSizeZ / -2) + distanceBetweenNodes / 2.0f;

        for (int z = 0; z < m_GridSizeZ; ++z)
        {
            for (int x = 0; x < m_GridSizeX; ++x)
            {
                if (m_NodeArray[x, z].isObstacle)
                {
                    Gizmos.color = Color.red;

                }
                else
                {
                    Gizmos.color = Color.green;
                }

                Vector3 position = bottomLeft + new Vector3(x * distanceBetweenNodes, 0.0f, z * distanceBetweenNodes);
                Gizmos.DrawSphere(transform.TransformPoint(position), nodeRadius);
            }
        }
    }
}
