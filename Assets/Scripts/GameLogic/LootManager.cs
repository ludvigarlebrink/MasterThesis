using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootManager : MonoBehaviour
{
    [SerializeField] private int m_LootAmount;

    private Loot[] m_LootPositions = null;
    private WorldManager m_WorldManager = null;
    private Button m_Button = null;

    private void DistributeLoot()
    {
        foreach (Loot loot in m_LootPositions)
        {
            loot.SetCollectable(false);
        }

        List<int> indexPool = new List<int>();
        List<int> indices = new List<int>();
        for (int i = 0; i < m_LootPositions.Length; ++i)
        {
            indexPool.Add(i);
        }
        int lootAmount = m_LootAmount <= m_LootPositions.Length ? m_LootAmount : m_LootPositions.Length;
        for (int i = 0; i < lootAmount; ++i)
        {
            int random = Random.Range(0, indexPool.Count);
            indices.Add(indexPool[random]);
            indexPool.RemoveAt(random);
        }
        foreach (int index in indices)
        {
            m_LootPositions[index].SetCollectable(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_WorldManager = FindObjectOfType<WorldManager>();
        if (!m_WorldManager)
        {
            Debug.LogError("No WorldManager in scene!");
            return;
        }

        m_LootPositions = GetComponentsInChildren<Loot>();
        if (m_LootPositions == null)
        {
            Debug.LogError("No Loot in children of " + name + "!");
            return;
        }

        m_Button = GetComponentInChildren<Button>();
        if (m_Button == null)
        {
            Debug.LogError("No Button in children of " + name + "!");
            return;
        }
        else
        {
            m_Button.onClick.AddListener(DistributeLoot);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
