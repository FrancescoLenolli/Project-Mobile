using UnityEngine;

public class PanelAnimator : MonoBehaviour
{
    private UIManager uiManager;
    private Vector3 originalPosition;
    private Vector3 targetPosition;

    public Transform panel;
    public Transform target;
    public float animationTime;

    private void Awake()
    {
        uiManager = UIManager.Instance;
        originalPosition = panel.localPosition;
        targetPosition = target.localPosition;
    }

    public void MoveToPosition()
    {
        bool isPanelVisible = IsPanelInView();

        Vector3 newPosition = isPanelVisible ? originalPosition : targetPosition;
        UIManager.Fade fadeType = isPanelVisible ? UIManager.Fade.Out : UIManager.Fade.In;

        uiManager.MoveRectObjectAndFade(panel, newPosition, animationTime, fadeType);
    }

    public void ShowPanel()
    {
        if (!IsPanelInView())
            uiManager.MoveRectObjectAndFade(panel, targetPosition, animationTime, UIManager.Fade.In);
    }

    public void HidePanel()
    {
        if (IsPanelInView())
            uiManager.MoveRectObjectAndFade(panel, originalPosition, animationTime, UIManager.Fade.Out);
    }

    public bool IsPanelInView()
    {
        return panel.localPosition == targetPosition;
    }
}
