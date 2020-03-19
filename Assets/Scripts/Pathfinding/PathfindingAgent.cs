using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingAgent : MonoBehaviour
{
    public PathfindingManager pathfindingManager = null;
    public float speed = 4.0f;

    private List<Vector3> m_Path;
    private int m_CurrentIndex = 0;
    private float m_Alpha = 0.0f;
    private bool m_HasPath = false;

    public bool SetDestination(Vector3 worldSpaceDestination)
    {
        if (pathfindingManager == null)
        {
            Debug.LogError("No pathfinding manager setup in pathfinding agent: " + name);
            return false;
        }

        m_CurrentIndex = 0;
        m_Alpha = 0.0f;

        m_HasPath = pathfindingManager.FindPath(transform.position, worldSpaceDestination, ref m_Path);
        if (m_HasPath)
        {
            m_CurrentIndex = 0;
            m_Alpha = 0.0f;
        }

        return m_HasPath;
    }

    private void Update()
    {
        if (!m_HasPath)
        {
            return;
        }

        float distance = Vector3.Distance(m_Path[m_CurrentIndex], m_Path[m_CurrentIndex + 1]);
        m_Alpha += (speed / distance) * Time.deltaTime;
        transform.position = Vector3.Lerp(m_Path[m_CurrentIndex], m_Path[m_CurrentIndex + 1], Mathf.Min(1.0f, m_Alpha));
        transform.rotation = Quaternion.LookRotation(Vector3.Normalize(m_Path[m_CurrentIndex] - m_Path[m_CurrentIndex + 1]), transform.up);
        
        if (m_Alpha >= 1.0f)
        {
            m_Alpha = 0.0f;
            ++m_CurrentIndex;

            if (m_CurrentIndex + 1 >= m_Path.Count)
            {
                m_CurrentIndex = 0;
                m_HasPath = false;
            }
        }
    }
}
