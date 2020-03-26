using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    public GameObject loot1Prefab;
    public GameObject loot5Prefab;
    public GameObject lootXPrefab;

    public Transform lootGrid;
    public RectTransform noiseBar;
    public Text nameField;

    private static float m_noiseBarMin = 0.2f;
    private static float m_noiseBarMax = 1.0f;

    private List<GameObject> m_loot1 = new List<GameObject>();
    private List<GameObject> m_loot5 = new List<GameObject>();
    private List<GameObject> m_lootX = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetName(string name)
    {
        nameField.text = name;
    }

    public void SetNoise(float noise)
    {
        float anchorY = (noise * (m_noiseBarMax - m_noiseBarMin)) + m_noiseBarMin;
        noiseBar.anchorMax = new Vector2(1, anchorY);
    }

    public void SetLoot(int loot)
    {
        int amount_x = Mathf.FloorToInt(loot / 10);
        int amount_5 = Mathf.FloorToInt((loot % 10) / 5);
        int amount_1 = loot % 5;

        PoolObject(lootXPrefab, m_lootX, lootGrid, 0, amount_x);
        PoolObject(loot5Prefab, m_loot5, lootGrid, amount_x, amount_5);
        PoolObject(loot1Prefab, m_loot1, lootGrid, amount_x + amount_5, amount_1);
    }

    private void PoolObject(GameObject prefab, List<GameObject> pool, Transform parent, int siblingIndex, int amount)
    {
        int count = 0;
        foreach (GameObject poolObject in pool)
        {
            if (poolObject.activeInHierarchy && poolObject.transform.parent == parent)
            {
                if (count < amount)
                {
                    poolObject.transform.SetSiblingIndex(siblingIndex);
                    ++count;
                }
                else
                {
                    poolObject.SetActive(false);
                }
            }
            else if (poolObject.transform.parent == parent)
            {
                if (count < amount)
                {
                    poolObject.SetActive(true);
                    poolObject.transform.SetSiblingIndex(siblingIndex);
                    ++count;
                }
            }
        }
        if (count >= amount)
        {
            return;
        }
        for (int i = count; i < amount; ++i)
        {
            GameObject newObject = Instantiate(prefab, parent);
            newObject.transform.SetSiblingIndex(siblingIndex);
            pool.Add(newObject);
        }
    }
}
