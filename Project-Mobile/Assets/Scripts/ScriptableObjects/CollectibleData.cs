using UnityEngine;

public class CollectibleData : ScriptableObject
{
    public int index;
    public Sprite icon;
    public new string name;
    [TextArea]
    public string description;
    public float costIncreaseMultiplier;
}
