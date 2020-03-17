using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PathfindingGrid))]
public class PathfindingGridInspector : Editor
{
    public override void OnInspectorGUI()
    {
        PathfindingGrid pathfindingGrid = (PathfindingGrid)target;

        pathfindingGrid.obstacleMask = EditorTools.LayerMaskField("Obstacle Mask", pathfindingGrid.obstacleMask);
        pathfindingGrid.gridWorldSizeX = EditorGUILayout.FloatField("Grid World Size X", pathfindingGrid.gridWorldSizeX);
        pathfindingGrid.gridWorldSizeZ = EditorGUILayout.FloatField("Grid World Size Z", pathfindingGrid.gridWorldSizeZ);
        pathfindingGrid.nodeRadius = EditorGUILayout.FloatField("Node Radius", pathfindingGrid.nodeRadius);
        pathfindingGrid.distanceBetweenNodes = EditorGUILayout.FloatField("Distance Between Nodes", pathfindingGrid.distanceBetweenNodes);

        if (GUILayout.Button("Generate"))
        {
            pathfindingGrid.GenerateGrid();
            EditorUtility.SetDirty(pathfindingGrid);
        }
    }
}
