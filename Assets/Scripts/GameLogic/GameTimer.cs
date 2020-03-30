using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameTimer : MonoBehaviour
{
    public UnityEvent onTimerFinished = new UnityEvent();

    private float m_CurrentTime = 0.0f;

    public string timeFormated
    {
        get
        {
            int minutes = Mathf.FloorToInt(m_CurrentTime / 60);
            int seconds = Mathf.FloorToInt(m_CurrentTime / 60);

            string value = "";
            if (minutes <= 9)
            {
                value += "0";
            }

            value += minutes.ToString();
            value += ":";
            
            if (seconds <= 9)
            {

            }

            return value;
        }
    }

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
