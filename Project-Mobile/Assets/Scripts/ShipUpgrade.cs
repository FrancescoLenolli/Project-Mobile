using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipUpgrade : MonoBehaviour
{
    public ShipUpgradeData shipUpgradeData = null;
    public Ship myShip = null;
    public int productionMultiplier = 0;
    public int cost = 0;
    // Was this Upgrade already bought?
    public bool isOwned = false;

    public Image imageIcon = null;
    public TextMeshProUGUI textName = null;
    public TextMeshProUGUI textProductionMultiplier = null;
    public TextMeshProUGUI textCost = null;
    public Button buttonBuy = null;

    private void SetValues(Ship ship)
    {
        myShip = ship;
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
