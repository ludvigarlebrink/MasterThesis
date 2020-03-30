using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Transform m_Point;
    [SerializeField] private ParticleSystem m_Particles;

    public Vector3 GetPosition()
    {
        if (m_Point == null)
        {
            Debug.LogError("No point setup in Loot component.");
            return Vector3.zero;
        }

        return m_Point.position;
    }

    private void Start()
    {
        if (m_Particles != null)
        {
            m_Particles.Play();
        }
    }
}
