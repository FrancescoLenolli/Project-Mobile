using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform targetLookAt = null;
    [SerializeField] private float positionOffset = 2.0f;

    private Vector3 originalPosition;
    private Vector3 direction;

    private void Awake()
    {
        originalPosition = transform.position;

        // direction pointing at the camera's back
        direction = -transform.forward;
    }

    private void LateUpdate()
    {
        transform.LookAt(targetLookAt, Vector3.up);
    }

    public void ChangePosition(int offset)
    {
        transform.position = originalPosition + (direction * (offset * positionOffset));
    }
}
