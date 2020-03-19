using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootCollector : MonoBehaviour
{
    public int m_LootCounter;
    public AreaManager manager;

    public void Setup(AreaManager _manager)
    {
        manager = _manager;

        if (manager)
        {
            manager.eventStartTimer += ResetLoot;
            manager.eventEndTimer += SendLootScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void IncreaseLoot(int increase)
    {
        m_LootCounter += increase;
    }

    public void ResetLoot()
    {
        m_LootCounter = 0;
    }

    private void SendLootScore()
    {
        manager.SetCurrentLoot(m_LootCounter);
    }
}
