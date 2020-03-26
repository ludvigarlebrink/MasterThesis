using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Transform m_Point;

    public Vector3 GetPosition()
    {
        if (m_Point == null)
        {
            Debug.LogError("No point setup in Loot component.");
            return Vector3.zero;
        }

        return transform.parent.InverseTransformPoint(m_Point.position);
    }
}
