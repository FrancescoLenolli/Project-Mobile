using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUpgrade : MonoBehaviour
{
    private ShipUpgradeData shipUpgradeData = null;
    private Ship myShip = null;
    private int productionMultiplier = 0;
    private int cost = 0;
    CurrencyManager currencyManager;

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

    private void SetValues(ShipUpgradeData newData)
    {
        shipUpgradeData = newData;

        // [!!!] UIManager with reference to CanvasBottom?
        myShip = FindObjectOfType<CanvasBottom>().panelShips.ReturnShipOfType(shipUpgradeData.shipType);

        imageIcon.sprite = shipUpgradeData.upgradeSprite;
        textName.text = shipUpgradeData.upgradeName;
        textProductionMultiplier.text = shipUpgradeData.productionMultiplier.ToString();
        cost = shipUpgradeData.cost;
        textCost.text = cost.ToString();
        buttonBuy = GetComponentInChildren<Button>();
    }

    public void Buy()
    {
        if (CurrencyManager.Instance.currency >= cost)
        {
            buttonBuy.interactable = false;
            myShip.UpdateProductionMultiplier(productionMultiplier);
            isOwned = true;

            // Make Sound
        }
        else
        {
            // Make Another Sound
        }
    }
}
