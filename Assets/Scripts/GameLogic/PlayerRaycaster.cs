using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    public LayerMask raycastMask;
    public event Action<Vector3, int> eventRaycastHit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, raycastMask) && eventRaycastHit != null)
            {
                eventRaycastHit.Invoke(hit.point, hit.collider.gameObject.layer);
            }
        }
    }
}
