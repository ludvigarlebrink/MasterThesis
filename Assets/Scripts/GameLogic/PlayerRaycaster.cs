using System;
using UnityEngine;

public class PlayerRaycaster : MonoBehaviour
{
    public LayerMask raycastMask;
    public event Action<Vector3, int> eventRaycastHit;

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
