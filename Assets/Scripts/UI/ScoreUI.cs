using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreUI : MonoBehaviour
{
    public List<GameObject> collectLoot;
    public Button button;
    public Text madeItToExitObject;

    public void Show(int collectedLoot, bool madeItToExit)
    {
        gameObject.SetActive(true);
        if (madeItToExit)
        {
            madeItToExitObject.text = "Successful Escaped";
        }
        else
        {
            madeItToExitObject.text = "Failed Escaped";
        }

        for (int i = 0; i < collectedLoot; ++i)
        {
            collectLoot[i].SetActive(true);
        }
    }

    private void OnClick()
    {
        foreach (GameObject item in collectLoot)
        {
            item.SetActive(false);
        }

        gameObject.SetActive(false);
    }

    private void Start()
    {
        button.onClick.AddListener(OnClick);
        gameObject.SetActive(false);

        foreach (GameObject item in collectLoot)
        {
            item.SetActive(false);
        }
    }
}
