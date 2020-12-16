using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour
{
    private float newValue;

    [Min(1)]
    public float rotationSpeed = 1;

    void FixedUpdate()
    {
        newValue += Time.fixedDeltaTime * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0, newValue, 0);
    }
}
