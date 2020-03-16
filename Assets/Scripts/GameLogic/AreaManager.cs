using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class AreaManager : MonoBehaviour
{

    public TrackableBehaviour trackableBehaviour;
    public GameObject startButton;
    public Text resultText;
    public Vector3 spawnPosition = new Vector3(-0.065f, 0.0f, -0.217f);
    public event Action eventStartTimer;
    public event Action eventEndTimer;

    [SerializeField] private Transform m_SpawnPosition;

    private PlayerRaycaster m_Raycaster;
    private Timer m_Timer;
    private float m_CurrentTimer = 0;
    private bool m_Running = false;
    private bool m_Tracking = false;
    private int m_CurrentNoise = 0;
    private int m_CurrentLoot = 0;
    private GameObject m_Burglar;

    public float remainingTimeFraction
    {
        get
        {
            return m_Timer.CurrentTime / m_CurrentTimer;
        }
    }

    public void IncreaseCurrentNoise(int increase)
    {
        m_CurrentNoise += increase;
    }


    public void StartButtonPressed()
    {
        if (/*m_tracking &&*/ !m_Running)
        {
            m_Burglar = PhotonNetwork.Instantiate("Prefabs/Gameplay/Burglar", m_SpawnPosition.position, Quaternion.identity);
            m_Burglar.GetComponent<NavFollow>().Setup(this, GetComponent<Pathfinding>());
            m_Burglar.GetComponent<LootCollector>().Setup(this);

            m_CurrentTimer = 3;
            m_Timer.InitializeTimer(m_CurrentTimer);
            m_Timer.EventTimerEnd += OnCountDownEnd;
            m_Timer.StartTimer();
            m_Running = true;

            m_CurrentNoise = 0;
            m_CurrentLoot = 0;

            SetStartButtonActive(false);
            resultText.text = "";
        }
    }

    public void StartButtonPressed(Vector3 hitPosition, int hitLayer)
    {
        if (LayerMask.NameToLayer("3DButton") == hitLayer)
        {
            StartButtonPressed();
        }
    }

    public void SetCurrentLoot(int loot)
    {
        m_CurrentLoot = loot;
    }

    private void Start()
    {
        m_Timer = GetComponent<Timer>();
        m_Raycaster = GetComponent<PlayerRaycaster>();

        if (trackableBehaviour)
        {
            trackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }

        if (startButton.activeInHierarchy && m_Raycaster)
        {
            m_Raycaster.eventRaycastHit += StartButtonPressed;
        }
    }

    private void OnCountDownEnd()
    {
        m_CurrentTimer = 30;
        m_Timer.InitializeTimer(m_CurrentTimer);
        m_Timer.EventTimerStart += OnGameTimeStart;
        m_Timer.EventTimerEnd += OnGameTimeEnd;
        m_Timer.StartTimer();
    }

    private void OnGameTimeEnd()
    {
        if (eventEndTimer != null)
        {
            eventEndTimer.Invoke();

            m_Running = false;
            resultText.text = "Loot Collected: " + m_CurrentLoot + "\nNoise Created: " + m_CurrentNoise;
            SetStartButtonActive(true);

            PhotonNetwork.Destroy(m_Burglar);
        }
    }

    private void OnGameTimeStart()
    {
        if (eventStartTimer != null)
        {
            eventStartTimer.Invoke();
        }
    }

    private void SetStartButtonActive(bool active)
    {
        m_Raycaster.eventRaycastHit -= StartButtonPressed;
        startButton.SetActive(active);
        if (active)
        {
            m_Raycaster.eventRaycastHit += StartButtonPressed;
        }
    }

    private void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult change)
    {
        if (!m_Tracking && (change.NewStatus == TrackableBehaviour.Status.DETECTED || change.NewStatus == TrackableBehaviour.Status.TRACKED))
        {
            m_Tracking = true;
            SetStartButtonActive(!m_Running);
        }
        else if (m_Tracking && change.NewStatus == TrackableBehaviour.Status.NO_POSE)
        {
            m_Tracking = false;
            SetStartButtonActive(false);
        }
    }
}
