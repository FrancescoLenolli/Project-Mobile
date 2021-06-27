using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelShipInfo : MonoBehaviour
{
    private CollectibleData currentData;
    private CanvasGroup canvasGroup;

    [SerializeField] private Image icon = null;
    [SerializeField] new private TextMeshProUGUI name = null;
    [SerializeField] private TextMeshProUGUI description = null;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void ShowInfo(CollectibleData data)
    {
        bool isItemDifferent = currentData != data;

        if (isItemDifferent)
        {
            ChangeVisibility(true);
            currentData = data;
            icon.sprite = data.icon;
            name.text = data.name;
            description.text = data.description;
        }
        else if (!isItemDifferent && IsPanelVisible())
        {
            ChangeVisibility(false);
        }
        else if (!(isItemDifferent || IsPanelVisible()))
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
        UtilsUI.ChangeVisibility(transform, canShowPanel);
    }

    private bool IsPanelVisible()
    {
        return canvasGroup.alpha == 1;
    }
}
