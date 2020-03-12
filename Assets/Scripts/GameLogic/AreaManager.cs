using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class AreaManager : MonoBehaviour
{
    private Timer m_timer;
    private float m_currentTimer = 0;
    private bool m_Running = false;
    public TrackableBehaviour trackableBehaviour;

    public float remainingTimeFraction
    {
        get
        {
            return m_timer.CurrentTime / m_currentTimer;
        }
    }

    public event Action EventStartTimer;
    public event Action EventEndTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = GetComponent<Timer>();

        if (trackableBehaviour)
        {
            trackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCountDownEnd()
    {
        m_currentTimer = 30;
        m_timer.InitializeTimer(m_currentTimer);
        m_timer.EventTimerStart += OnGameTimeStart;
        m_timer.EventTimerEnd += OnGameTimeEnd;
        m_timer.StartTimer();
    }

    private void OnGameTimeStart()
    {
        if (EventStartTimer != null )
        {
            EventStartTimer.Invoke();
        }
    }

    private void OnGameTimeEnd()
    {
        if (EventEndTimer != null)
        {
            EventEndTimer.Invoke();
        }
    }

    private void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult change)
    {
        if ( !m_Running && (change.NewStatus == TrackableBehaviour.Status.DETECTED || change.NewStatus == TrackableBehaviour.Status.TRACKED))
        {
            m_currentTimer = 3;
            m_timer.InitializeTimer(m_currentTimer);
            m_timer.EventTimerEnd += OnCountDownEnd;
            m_timer.StartTimer();
            m_Running = true;
        }
    }
}
