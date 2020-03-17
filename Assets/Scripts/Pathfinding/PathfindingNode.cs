using UnityEngine;

[System.Serializable]
public class PathfindingNode
{
    public int gridX;
    public int gridZ;
    public bool isObstacle;
    public Vector3 position;
    public PathfindingNode parentNode;

    public int gCost;
    public int hCost;

    public int fCost
    { 
        get 
        {
            return gCost + hCost;
        }
    }

    public PathfindingNode(bool isWall, Vector3 position, int gridX, int gridZ)
    {
        this.isObstacle = isWall;
        this.position = position;
        this.gridX = gridX;
        this.gridZ = gridZ;
    }
}

