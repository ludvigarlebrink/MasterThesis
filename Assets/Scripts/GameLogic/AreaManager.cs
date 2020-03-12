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
    private bool m_tracking = false;
    public TrackableBehaviour trackableBehaviour;

    public GameObject startButton;
    public Text resultText;

    private int m_currentNoise = 0;
    private int m_currentLoot = 0;

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

            m_Running = false;
            resultText.text = "Loot Collected: " + m_currentLoot + "\nNoise Created: " + m_currentNoise;
            startButton.SetActive(true);
        }
    }

    private void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult change)
    {
        if (!m_tracking && (change.NewStatus == TrackableBehaviour.Status.DETECTED || change.NewStatus == TrackableBehaviour.Status.TRACKED))
        {
            m_tracking = true;
            startButton.SetActive(!m_Running);
        }
        else if (m_tracking && change.NewStatus == TrackableBehaviour.Status.NO_POSE)
        {
            m_tracking = false;
            startButton.SetActive(false);
        }
    }

    public void StartButtonPressed()
    {
        if (m_tracking && !m_Running)
        {
            m_currentTimer = 3;
            m_timer.InitializeTimer(m_currentTimer);
            m_timer.EventTimerEnd += OnCountDownEnd;
            m_timer.StartTimer();
            m_Running = true;

            m_currentNoise = 0;
            m_currentLoot = 0;

            startButton.SetActive(false);
            resultText.text = "";
        }
    }

    public void IncreaseCurrentNoise(int increase)
    {
        m_currentNoise += increase;
    }

    public void SetCurrentLoot(int loot)
    {
        m_currentLoot = loot;
    }
}
