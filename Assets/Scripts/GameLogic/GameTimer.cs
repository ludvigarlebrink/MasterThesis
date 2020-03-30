using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    public UnityEvent onTimerFinished = new UnityEvent();

    private float m_CurrentTime = 0.0f;

    public void StartTimer(float startTime)
    {
        m_CurrentTime = startTime;
    }

    private void Update()
    {
        if (m_CurrentTime > 0.0f)
        {
            m_CurrentTime -= Time.deltaTime;

            if (m_CurrentTime <= 0.0f)
            {
                onTimerFinished.Invoke();
            }
        }
    }
}
