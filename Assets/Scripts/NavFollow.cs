using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NavFollow : MonoBehaviour
{
    public Pathfinding Pathfinder;
    public LayerMask RaycastMask;

    bool m_stopped = true;
    float m_baseSpeed = 0.5f;
    List<Vector3> m_cornerNodes;
    int m_cornersIterator = 0;

    void Start()
    {
        Stop();
    }

    void Update()
    {
        if (m_stopped)
        {
          //  return;
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, RaycastMask))
            {
                m_cornerNodes = Pathfinder.FindPath(transform.position, hit.point);
                m_cornersIterator = 0;
            }
        }

        if (m_cornersIterator < m_cornerNodes.Count && m_cornerNodes != null && m_cornerNodes.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_cornerNodes[m_cornersIterator], m_baseSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_cornerNodes[m_cornersIterator]) < Vector3.kEpsilon)
            {
                m_cornersIterator++;
            }
        }
    }

    public void Go()
    {
        m_stopped = false;
    }

    public void Stop()
    {
        m_stopped = true;
    }

    public void SetSpeedModifier(float modifier, float seconds)
    {
        StartCoroutine(ResetSpeedCoroutine(seconds));
    }

    private IEnumerator ResetSpeedCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);

    }
}
