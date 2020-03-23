using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    bool m_Collected;
    Color m_ActiveColor;

    public AreaManager manager;
    public int amount = 1;

    // Start is called before the first frame update
    void Start()
    {
        m_Collected = false;
        m_ActiveColor = GetComponent<MeshRenderer>().material.color;

        if (manager)
        {
            manager.eventStartTimer += ResetLoot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_Collected)
        {
            LootCollector collector = other.GetComponentInParent<LootCollector>();
            if (collector)
            {
                collector.IncreaseLoot(amount);
                m_Collected = true;
                GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
    }

    private void ResetLoot()
    {
        m_Collected = false;
        GetComponent<MeshRenderer>().material.color = m_ActiveColor;
    }
}
