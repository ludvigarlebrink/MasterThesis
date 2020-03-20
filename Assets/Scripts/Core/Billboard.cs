using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    public Transform sceneCameraTransform = null;

    private void Start()
    {
        if (!sceneCameraTransform)
        {
            sceneCameraTransform = Camera.main.transform;
        }
    }

    private void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(sceneCameraTransform.forward, sceneCameraTransform.up);
    }
}
