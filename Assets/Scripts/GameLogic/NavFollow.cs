using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class NavFollow : MonoBehaviour
{
    public Pathfinding pathfinder;
    public LayerMask raycastMask;
    public AreaManager manager;
    public SpriteMask timerMask;


    bool m_Stopped = true;
    float m_BaseSpeed = 0.5f;
    List<Vector3> m_CornerNodes;
    int m_CornersIterator = 0;

    float m_SpeedModifier = 1.0f;

    void Start()
    {
        if (manager)
        {
            manager.EventStartTimer += Go;
            manager.EventEndTimer += Stop;
        }

        Stop();
    }

    void Update()
    {
        if (timerMask && manager)
        {
            timerMask.alphaCutoff = manager.remainingTimeFraction;
        }

        if (m_Stopped)
        {
            return;
        }

        if (Input.GetMouseButton(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, raycastMask))
            {
                m_CornerNodes = pathfinder.FindPath(transform.position, hit.point);
                m_CornersIterator = 0;
            }
        }

        if (m_CornerNodes != null && m_CornersIterator < m_CornerNodes.Count && m_CornerNodes.Count > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, m_CornerNodes[m_CornersIterator], m_BaseSpeed * m_SpeedModifier * Time.deltaTime);
            if (Vector3.Distance(transform.position, m_CornerNodes[m_CornersIterator]) < Vector3.kEpsilon)
            {
                m_CornersIterator++;
            }
        }
    }

    public void Go()
    {
        m_Stopped = false;
    }

    public void Stop()
    {
        m_Stopped = true;
    }

    public void SetSpeedModifier(float modifier, float seconds)
    {
        if (modifier <= 0)
        {
            modifier = float.Epsilon;
        }
        StartCoroutine(ModifySpeedCoroutine(modifier, seconds));
    }

    private IEnumerator ModifySpeedCoroutine(float modifier, float seconds)
    {
        m_SpeedModifier *= modifier;
        yield return new WaitForSeconds(seconds);
        m_SpeedModifier /= modifier;
    }
}
