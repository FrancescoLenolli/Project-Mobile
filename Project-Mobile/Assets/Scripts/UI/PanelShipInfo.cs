using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelShipInfo : MonoBehaviour
{
    private UIManager uiManager;
    private ShipData currentData;
    private CanvasGroup canvasGroup;

    [SerializeField] private Image icon = null;
    [SerializeField] new private TextMeshProUGUI name = null;
    [SerializeField] private TextMeshProUGUI description = null;

    private void Start()
    {
        uiManager = UIManager.Instance;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowInfo(ShipData data)
    {
        bool isShipDifferent = currentData != data;

        if (isShipDifferent)
        {
            ChangeVisibility(true);
            currentData = data;
            icon.sprite = data.icon;
            name.text = data.name;
            description.text = data.description;
        }
        else if (!isShipDifferent && IsPanelVisible())
        {
            ChangeVisibility(false);
        }
        else if (!(isShipDifferent || IsPanelVisible()))
        {
            ChangeVisibility(true);
        }
    }

    public void HidePanel()
    {
        ChangeVisibility(false);
    }

    private void ChangeVisibility(bool canShowPanel)
    {
        uiManager.ChangeVisibility(transform, canShowPanel);
    }

    private bool IsPanelVisible()
    {
        return canvasGroup.alpha == 1;
    }
}
