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

    }

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
