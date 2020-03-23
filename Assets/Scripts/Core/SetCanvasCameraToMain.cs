using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetCanvasCameraToMain : MonoBehaviour
{
    private void Start()
    {
        Camera cam = Camera.main;
        Canvas canvas = GetComponent<Canvas>();
        if (canvas && cam)
        {
            canvas.worldCamera = cam;
        }
    }
}
