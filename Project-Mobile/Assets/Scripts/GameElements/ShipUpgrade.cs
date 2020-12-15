using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public delegate void UpgradeBought(int myCurrencyGainModifier);
public delegate void ChangeOwnedStatus(ShipUpgradeData myData);
public delegate void UpgradeDestroyed(Transform transform);
public class ShipUpgrade : MonoBehaviour
{
    public event UpgradeBought EventBoughtUpgrade;
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
    public TextMeshProUGUI textDescription = null;
    public TextMeshProUGUI textCost = null;
    public Button buttonBuy = null;
    public List<Sprite> listButtonSprites = new List<Sprite>();

    private void Awake()
    {
        currencyManager = CurrencyManager.Instance;
    }

    private void Update()
    {
        if (currencyManager)
        {
            bool canBuy = currencyManager.currency >= cost;
            buttonBuy.image.sprite = canBuy ? listButtonSprites[0] : listButtonSprites[1];
            buttonBuy.interactable = canBuy;
        }
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
        textDescription.text = $"{newData.description}\nImprove efficiency by {productionMultiplier}%.";
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
