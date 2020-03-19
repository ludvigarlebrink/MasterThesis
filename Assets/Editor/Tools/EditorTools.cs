using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditorTools
{
    public static LayerMask LayerMaskField(string label, LayerMask selected)
    {
        List<string> layers = new List<string>();
        string[] layerNames = new string[4];

        for (int i = 0; i < 32; i++)
        {
            layers.Add(i.ToString() + ": " + LayerMask.LayerToName(i));
        }

        if (layerNames.Length != layers.Count)
        {
            layerNames = new string[layers.Count];
        }

        for (int i = 0; i < layerNames.Length; i++)
        {
            layerNames[i] = layers[i];
        }

        selected.value = EditorGUILayout.MaskField(label, selected.value, layerNames);

        return selected;
    }
}
