using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private GameObject m_TimerObject = null;
    [SerializeField] private Text m_TimerText = null;

    private GameTimer m_GameTimer = null;

    public GameTimer gameTimer
    {
        set
        {
            m_GameTimer = value;
        }
    }

    private void Update()
    {
        if (m_GameTimer == null)
        {
            return;
        }

        if (m_GameTimer.currentTime > 0.0f)
        {
            m_TimerObject.SetActive(true);
            m_TimerText.text = m_GameTimer.timeFormated;
        }
        else
        {
            m_TimerObject.SetActive(false);
        }
    }
}
