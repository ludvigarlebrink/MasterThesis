using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollector : MonoBehaviour
{
    public int m_lootCounter;
    public AreaManager manager;

    // Start is called before the first frame update
    void Start()
    {
        if (manager)
        {
            manager.EventStartTimer += ResetLoot;
            manager.EventEndTimer += SendLootScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseLoot(int increase)
    {
        m_lootCounter += increase;
    }

    public void ResetLoot()
    {
        m_lootCounter = 0;
    }

    private void SendLootScore()
    {
        manager.SetCurrentLoot(m_lootCounter);
    }
}
