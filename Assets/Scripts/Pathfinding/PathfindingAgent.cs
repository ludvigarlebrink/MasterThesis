using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PathfindingAgent : MonoBehaviour
{
    public PathfindingManager pathfindingManager = null;
    public float speed = 4.0f;
    public UnityEvent onReachedDestination;
    public UnityEvent onPathCancelled;

    private List<Vector3> m_Path;
    private int m_CurrentIndex = 0;
    private float m_Alpha = 0.0f;
    private bool m_HasPath = false;
    private bool m_Local = false;

    public bool SetDestination(Vector3 worldSpaceDestination, bool local = false)
    {
        if (pathfindingManager == null)
        {
            Debug.LogError("No pathfinding manager setup in pathfinding agent: " + name);
            return false;
        }

        m_CurrentIndex = 0;
        m_Alpha = 0.0f;

        m_HasPath = pathfindingManager.FindPath(transform.position, worldSpaceDestination, ref m_Path, local);
        if (m_HasPath)
        {
            m_Local = local;
            m_CurrentIndex = 0;
            m_Alpha = 0.0f;
        }
        // Added by Chris to enable looting when already standing near object
        else if (pathfindingManager.GetNode(transform.position) == pathfindingManager.GetNode(worldSpaceDestination))
        {
            onReachedDestination.Invoke();
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
        
        if (m_Local)
        {
            transform.localPosition = Vector3.Lerp(m_Path[m_CurrentIndex], m_Path[m_CurrentIndex + 1], Mathf.Min(1.0f, m_Alpha));
        }
        else
        {
            transform.position = Vector3.Lerp(m_Path[m_CurrentIndex], m_Path[m_CurrentIndex + 1], Mathf.Min(1.0f, m_Alpha));
        }

        if (m_Local)
        {
            transform.localRotation = Quaternion.LookRotation(Vector3.Normalize(m_Path[m_CurrentIndex] - m_Path[m_CurrentIndex + 1]));
        }
        else
        {
            transform.rotation = Quaternion.LookRotation(Vector3.Normalize(m_Path[m_CurrentIndex] - m_Path[m_CurrentIndex + 1]));
        }

        if (m_Alpha >= 1.0f)
        {
            m_Alpha = 0.0f;
            ++m_CurrentIndex;

            if (m_CurrentIndex + 1 >= m_Path.Count)
            {
                m_CurrentIndex = 0;
                m_HasPath = false;
                onReachedDestination.Invoke();
            }
        }
    }
}
