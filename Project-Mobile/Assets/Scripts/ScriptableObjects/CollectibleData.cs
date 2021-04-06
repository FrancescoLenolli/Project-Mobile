using UnityEngine;

public class CollectibleData : ScriptableObject
{
    public int index;
    public Sprite icon;
    public new string name;
    [TextArea]
    public string description;
    [Tooltip("How much the cost of this Collectible is increased after buying a unit of it.")]
    public float costIncreaseMultiplier;
}
