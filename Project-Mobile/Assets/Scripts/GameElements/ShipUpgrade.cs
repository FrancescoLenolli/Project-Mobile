using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void BoughtUpgrade(int myCurrencyGainModifier);
public delegate void ChangeOwnedStatus(ShipUpgradeData myData);
public delegate void UpgradeDestroyed(Transform transform);
public class ShipUpgrade : MonoBehaviour
{
    public event BoughtUpgrade EventBoughtUpgrade;
    public event ChangeOwnedStatus EventChangeOwnedStatus;
    public event UpgradeDestroyed EventUpgradeDestroyed;


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

    public void SetValues(ShipUpgradeData newData, PanelShipsUpgrades newPanel)
    {
        shipUpgradeData = newData;
        PanelShipsUpgrades panelShipsUpgrades = newPanel;

        // TODO: UIManager with reference to CanvasBottom?
        myShip = FindObjectOfType<CanvasBottom>().panelShips.ReturnShipOfType(shipUpgradeData.shipType);

        EventBoughtUpgrade += myShip.UpdateIdleGain;
        EventChangeOwnedStatus += panelShipsUpgrades.SetOwnedStatus;
        EventUpgradeDestroyed += panelShipsUpgrades.ResizeContainer;

        cost = shipUpgradeData.cost;
        productionMultiplier = shipUpgradeData.productionMultiplier;

        imageIcon.sprite = shipUpgradeData.upgradeSprite;
        textName.text = shipUpgradeData.upgradeName;
        textProductionMultiplier.text = productionMultiplier.ToString();
        textCost.text = Formatter.FormatValue(cost);
        buttonBuy = GetComponentInChildren<Button>();
    }

    public void Buy()
    {
        if (!isOwned)
        {
            if (currencyManager.currency >= cost)
            {
                currencyManager.currency -= cost;

                EventBoughtUpgrade?.Invoke(productionMultiplier);
                EventChangeOwnedStatus?.Invoke(shipUpgradeData);
                isOwned = true;

                buttonBuy.interactable = false;

                // Make Sound

                EventUpgradeDestroyed?.Invoke(transform);

                Destroy(gameObject);
            }
            else
            {
                // Make Another Sound
            }
        }
    }
}
