using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotator : MonoBehaviour
{
    public float speed = 100;

    private void Update()
    {
        transform.Rotate(0.0f, 0.0f, speed * Time.deltaTime);
    }
}
