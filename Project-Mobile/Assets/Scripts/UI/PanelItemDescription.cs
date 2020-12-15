using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PanelItemDescription : MonoBehaviour
{
    private UIManager uiManager = null;
    private Vector3 originalPosition = Vector3.zero;

    public Transform newTargetPosition = null;
    public float animationTime = 0.2f;
    [Space(10)]
    public Image imageItem = null;
    public TextMeshProUGUI textItemName = null;
    public TextMeshProUGUI textItemDescription = null;

    private void Awake()
    {
        uiManager = UIManager.Instance;
        originalPosition = transform.localPosition;
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
        uiManager.MoveRectObject(animationTime, transform, originalPosition);
    }
}
