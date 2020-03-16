using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public float speedModifier = 0.1f;
    public float modifyTime = 5.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            NavFollow follow = other.GetComponentInParent<NavFollow>();
            if (follow)
            {
                follow.SetSpeedModifier(speedModifier, modifyTime);
            }
        }
    }
}
