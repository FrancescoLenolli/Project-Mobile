using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PanelItemDescription : MonoBehaviour
{
    private UIManager uiManager = null;

    public Transform newTargetPosition = null;
    public float animationTime = 0.2f;
    [Space(10)]
    public Image imageItem = null;
    public TextMeshProUGUI textItemName = null;
    public TextMeshProUGUI textItemDescription = null;

    private void Awake()
    {
        uiManager = UIManager.Instance;
    }

    public void ShowPanel(Sprite itemImage, string itemName, string itemDescription)
    {
        imageItem.sprite = itemImage;
        textItemName.text = itemName;
        textItemDescription.text = itemDescription;
        uiManager.MoveRectObject(animationTime, transform, newTargetPosition);
    }

    public void HidePanel()
    {
        //uiManager.MoveRectObject(animationTime, transform, originalPosition);
    }
}
