using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float SpeedModifier = 0.1f;
    public float ModifyTime = 5.0f;

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
                follow.SetSpeedModifier(SpeedModifier, ModifyTime);
            }
        }
    }
}
