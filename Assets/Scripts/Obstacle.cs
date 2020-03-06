using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NavFollow follow = other.GetComponentInParent<NavFollow>();
            if (follow)
            {
                follow.SetSpeedModifier(0.1f, 5f);
            }
        }
    }
}
