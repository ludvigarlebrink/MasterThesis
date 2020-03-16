using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public event Action EventTimerStart;
    public event Action EventTimerEnd;
    public event Action EventTimerAbort;

    private float m_Time = 0;
    private bool m_Abort = false;

    public float CurrentTime
    {
        get
        {
            return m_Time;
        }
    }

    public void AbortTimer()
    {
        m_Abort = true;
    }

    public void InitializeTimer(float seconds)
    {
        m_Time = seconds;

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

    private IEnumerator TimerCoroutine()
    {
        while (m_Time > 0)
        {
            yield return new WaitForEndOfFrame();
            m_Time -= Time.deltaTime;

            if (m_Abort)
            {
                if (EventTimerAbort != null)
                {
                    EventTimerAbort.Invoke();
                }
                m_Abort = false;
                yield break;
            }
        }

        if (m_Time < 0)
        {
            m_Time = 0;
        }

        if (EventTimerEnd != null)
        {
            EventTimerEnd.Invoke();
        }
    }
}
