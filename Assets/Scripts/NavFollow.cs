using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavFollow : MonoBehaviour
{
    NavMeshAgent m_agent = null;
    bool m_stopped = true;
    float m_baseSpeed = 0.5f;

    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        Stop();
    }

    void Update()
    {
        if (m_stopped)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                m_agent.destination = hit.point;
            }
        }
    }

    public void Go()
    {
        m_stopped = false;
        m_agent.speed = m_baseSpeed;
    }

    public void Stop()
    {
        m_stopped = true;
        m_agent.speed = 0;
    }

    public void SetSpeedModifier(float modifier, float seconds)
    {
        m_agent.speed = m_baseSpeed * modifier;
        StartCoroutine(ResetSpeedCoroutine(seconds));
    }

    private IEnumerator ResetSpeedCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        m_agent.speed = m_baseSpeed;
    }
}
