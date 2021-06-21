using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform targetLookAt = null;

    private Vector3 originalPosition;
    private Vector3 direction;

    private void Awake()
    {
        originalPosition = transform.position;

        // direction pointing at the camera's back
        direction = transform.position - transform.forward;
    }

    private void LateUpdate()
    {
        transform.LookAt(targetLookAt, Vector3.up);
    }

    public void ChangePosition(int offset)
    {
        transform.position = originalPosition + (direction * (offset * 0.5f));
    }
}
