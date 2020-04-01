using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    [SerializeField] private Transform m_Point;
    [SerializeField] private ParticleSystem m_Particles;
    [SerializeField] private ParticleSystem m_CollectedEffect;

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
        if (m_CollectedEffect != null)
        {
            m_CollectedEffect.Play();
        }
        SetCollectable(false);
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

    public void SetCollectable(bool collectable)
    {
        Collectable = collectable;
        if (collectable)
        {
            m_Particles.Play();
        }
        else
        {
            m_Particles.Stop();
            m_Particles.Clear();
        }
    }

    private void Start()
    {
        Collectable = false;
    }
}
