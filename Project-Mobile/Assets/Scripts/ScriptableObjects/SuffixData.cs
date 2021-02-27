using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomData/Suffixes", fileName = "New Suffixes")]
public class SuffixData : ScriptableObject
{
    public List<string> suffixes;
}
