using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vuforia;

public class AreaManager : MonoBehaviour
{
    private PlayerRaycaster m_raycaster;
    private Timer m_timer;
    private float m_currentTimer = 0;
    private bool m_Running = false;
    private bool m_tracking = false;
    public TrackableBehaviour trackableBehaviour;

    public GameObject startButton;
    public Text resultText;

    private int m_currentNoise = 0;
    private int m_currentLoot = 0;

    private GameObject m_burglar;

    public float remainingTimeFraction
    {
        get
        {
            return m_timer.CurrentTime / m_currentTimer;
        }
    }

    public event Action eventStartTimer;
    public event Action eventEndTimer;

    // Start is called before the first frame update
    void Start()
    {
        m_timer = GetComponent<Timer>();
        m_raycaster = GetComponent<PlayerRaycaster>();

        if (trackableBehaviour)
        {
            trackableBehaviour.RegisterOnTrackableStatusChanged(OnTrackableStatusChanged);
        }

        if (startButton.activeInHierarchy && m_raycaster)
        {
            m_raycaster.eventRaycastHit += StartButtonPressed;
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
        if (eventStartTimer != null)
        {
            eventStartTimer.Invoke();
        }
    }

    private void OnGameTimeEnd()
    {
        if (eventEndTimer != null)
        {
            eventEndTimer.Invoke();

            m_Running = false;
            resultText.text = "Loot Collected: " + m_currentLoot + "\nNoise Created: " + m_currentNoise;
            SetStartButtonActive(true);

            PhotonNetwork.Destroy(m_burglar);
        }
    }

    private void OnTrackableStatusChanged(TrackableBehaviour.StatusChangeResult change)
    {
        if (!m_tracking && (change.NewStatus == TrackableBehaviour.Status.DETECTED || change.NewStatus == TrackableBehaviour.Status.TRACKED))
        {
            m_tracking = true;
            SetStartButtonActive(!m_Running);
        }
        else if (m_tracking && change.NewStatus == TrackableBehaviour.Status.NO_POSE)
        {
            m_tracking = false;
            SetStartButtonActive(false);
        }
    }

    public void StartButtonPressed()
    {
        if (/*m_tracking &&*/ !m_Running)
        {
            m_burglar = PhotonNetwork.Instantiate("Prefabs/Gameplay/Burglar", transform.parent.TransformPoint(new Vector3(-0.065f, 0.0f, -0.217f)), Quaternion.identity);
            m_burglar.GetComponent<NavFollow>().Setup(this, GetComponent<Pathfinding>());
            m_burglar.GetComponent<LootCollector>().Setup(this);

            m_currentTimer = 3;
            m_timer.InitializeTimer(m_currentTimer);
            m_timer.EventTimerEnd += OnCountDownEnd;
            m_timer.StartTimer();
            m_Running = true;

            m_currentNoise = 0;
            m_currentLoot = 0;

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

    public void IncreaseCurrentNoise(int increase)
    {
        m_currentNoise += increase;
    }

    public void SetCurrentLoot(int loot)
    {
        m_currentLoot = loot;
    }

    private void SetStartButtonActive(bool active)
    {
        m_raycaster.eventRaycastHit -= StartButtonPressed;
        startButton.SetActive(active);
        if (active)
        {
            m_raycaster.eventRaycastHit += StartButtonPressed;
        }
    }
}
