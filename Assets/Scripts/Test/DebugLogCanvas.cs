using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogCanvas : MonoBehaviour
{
    public Transform contentList;
    public GameObject debugLogPrefab;
    public GameObject toggleObject;
    public Text buttonText;

    // Start is called before the first frame update
    void Start()
    {
        Application.logMessageReceived += DebugCallBack;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DebugCallBack(string condition, string stacktrace, UnityEngine.LogType type)
    {
        GameObject logMessage = Instantiate(debugLogPrefab, contentList);
        logMessage.GetComponentInChildren<Text>().text = condition + "\n" + stacktrace;
        Color background = (type == LogType.Warning ? Color.yellow : (type == LogType.Error ? Color.red : Color.white));
        background.a = 0.4f;
        logMessage.GetComponent<Image>().color = background;
    }

    public void ToggleScrollView()
    {
        toggleObject.SetActive(!toggleObject.activeInHierarchy);
        buttonText.text = toggleObject.activeInHierarchy ? "Hide" : "Show";
    }
}
