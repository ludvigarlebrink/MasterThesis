using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : MonoBehaviour
{
    Timer m_timer;
    float m_currentTimer = 0;

    public NavFollow AvatarNavFollow;
    public SpriteMask TimerMask;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = GetComponent<Timer>();

        m_currentTimer = 3;
        m_timer.InitializeTimer(m_currentTimer);
        m_timer.EventTimerEnd += OnCountDownEnd;
        m_timer.StartTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_currentTimer > 0)
        {
            TimerMask.alphaCutoff = m_timer.CurrentTime / m_currentTimer;
        }
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
        if (AvatarNavFollow)
        {
            AvatarNavFollow.Go();
        }
    }

    private void OnGameTimeEnd()
    {
        if (AvatarNavFollow)
        {
            AvatarNavFollow.Stop();
        }
    }
}
