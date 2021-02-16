using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Upgrade : MonoBehaviour
{
    private CurrencyManager currencyManager;
    private UIManager uiManager;
    private UpgradeData upgradeData;
    private Ship ship;
    private Transform container;

    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textDescription = null;
    public TextMeshProUGUI textCost = null;
    public Image imageIcon = null;

    public void InitData(UpgradeData upgradeData, Ship ship, Transform container)
    {
        currencyManager = CurrencyManager.Instance;
        uiManager = UIManager.Instance;

        this.upgradeData = upgradeData;
        this.container = container;
        this.ship = ship;

        textName.text = upgradeData.name;
        textDescription.text = $"Increase {ship.name} currency gain by {upgradeData.upgradePercentage}%";
        textCost.text = Formatter.FormatValue(upgradeData.cost);
        imageIcon.sprite = upgradeData.icon;
    }

    public void Buy()
    {
        if (CanBuy() || GameManager.Instance.isTesting)
        {
            if (!GameManager.Instance.isTesting)
                currencyManager.RemoveCurrency(upgradeData.cost);

            ship.UpgradeBought(upgradeData);
            uiManager.ResizeContainer(transform, container, UIManager.Resize.Subtract);
            Destroy(gameObject);
        }
    }

    private bool CanBuy()
    {
        return upgradeData.cost <= CurrencyManager.Instance.currency;
    }
}
