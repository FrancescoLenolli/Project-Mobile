using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private Vector3 originalPosition;

    [SerializeField] private Transform targetLookAt = null;

    private void Awake()
    {
        originalPosition = transform.localPosition;
    }

    private void LateUpdate()
    {
        transform.LookAt(targetLookAt, Vector3.up);
    }

    public void ChangePosition(int offset)
    {
        transform.localPosition = new Vector3(originalPosition.x, originalPosition.y, originalPosition.z - offset);
    }
}
