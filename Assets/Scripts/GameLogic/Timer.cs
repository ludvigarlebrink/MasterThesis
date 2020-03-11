using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    float m_time = 0;
    bool m_abort = false;

    public float CurrentTime
    {
        get
        {
            return m_time;
        }
        set
        {
            return;
        }
    }

    public event Action EventTimerStart;
    public event Action EventTimerEnd;
    public event Action EventTimerAbort;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void InitializeTimer(float seconds)
    {
        m_time = seconds;

        EventTimerStart = null;
        EventTimerEnd = null;
        EventTimerAbort = null;
    }

    public void StartTimer()
    {
        if (EventTimerStart != null)
        {
            EventTimerStart.Invoke();
        }

        StartCoroutine(TimerCoroutine());
    }

    public void AbortTimer()
    {
        m_abort = true;
    }

    private IEnumerator TimerCoroutine()
    {
        while (m_time > 0)
        {
            yield return new WaitForEndOfFrame();
            m_time -= Time.deltaTime;

            if (m_abort)
            {
                if (EventTimerAbort != null)
                {
                    EventTimerAbort.Invoke();
                }
                m_abort = false;
                yield break;
            }
        }

        if (m_time < 0)
        {
            m_time = 0;
        }

        if (EventTimerEnd != null)
        {
            EventTimerEnd.Invoke();
        }
    }
}
