using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseSource : MonoBehaviour
{
    public AreaManager manager;
    public int noiseIncrease;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.IncreaseCurrentNoise(noiseIncrease);
        }
    }
}
