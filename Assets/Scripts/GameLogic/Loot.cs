using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Transform m_Point;
    [SerializeField] private ParticleSystem m_Particles;
    // Added by Chris to deactivate loot after collecting
    public bool Collectable
    {
        get;
        private set;
    }

    public bool Collect()
    {
        if (!Collectable)
        {
            return false;
        }
        m_Particles.Stop();
        m_Particles.Clear();
        Collectable = false;
        return true;
    }

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
        Collectable = true;
    }
}
