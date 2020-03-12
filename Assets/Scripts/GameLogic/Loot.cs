using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    bool m_collected;
    Color m_activeColor;

    public AreaManager manager;

    // Start is called before the first frame update
    void Start()
    {
        m_collected = false;
        m_activeColor = GetComponent<MeshRenderer>().material.color;

        if (manager)
        {
            manager.EventStartTimer += ResetLoot;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !m_collected)
        {
            LootCollector collector = other.GetComponentInParent<LootCollector>();
            if (collector)
            {
                collector.IncreaseLoot(1);
                m_collected = true;
                GetComponent<MeshRenderer>().material.color = Color.gray;
            }
        }
    }

    private void ResetLoot()
    {
        m_collected = false;
        GetComponent<MeshRenderer>().material.color = m_activeColor;
    }
}
