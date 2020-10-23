using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void BoughtUpgrade(int currencyGainModifier);
public class ShipUpgrade : MonoBehaviour
{
    public event BoughtUpgrade eventBoughtUpgrade;

    private ShipUpgradeData shipUpgradeData = null;
    private Ship myShip = null;
    private int productionMultiplier = 0;
    private int cost = 0;
    private CurrencyManager currencyManager;

    // Was this Upgrade already bought?
    [HideInInspector] public bool isOwned = false;

    public Image imageIcon = null;
    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textProductionMultiplier = null;
    public TextMeshProUGUI textCost = null;
    public Button buttonBuy = null;

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Update()
    {
        buttonBuy.interactable = currencyManager.currency >= cost ? true : false;
    }

    public void SetValues(ShipUpgradeData newData)
    {
        shipUpgradeData = newData;

        // [!!!] UIManager with reference to CanvasBottom?
        myShip = FindObjectOfType<CanvasBottom>().panelShips.ReturnShipOfType(shipUpgradeData.shipType);

        eventBoughtUpgrade += myShip.UpdateIdleGain;

        cost = shipUpgradeData.cost;
        productionMultiplier = shipUpgradeData.productionMultiplier;

        imageIcon.sprite = shipUpgradeData.upgradeSprite;
        textName.text = shipUpgradeData.upgradeName;
        textProductionMultiplier.text = productionMultiplier.ToString();
        textCost.text = cost.ToString();
        buttonBuy = GetComponentInChildren<Button>();
    }

    public void Buy()
    {
        if (!isOwned)
        {
            if (currencyManager.currency >= cost)
            {
                currencyManager.currency -= cost;

                buttonBuy.interactable = false;
                eventBoughtUpgrade?.Invoke(productionMultiplier);
                isOwned = true;

                // Make Sound
            }
            else
            {
                // Make Another Sound
            }
        }
    }
}
