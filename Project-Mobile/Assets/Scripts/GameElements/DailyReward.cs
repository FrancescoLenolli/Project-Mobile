using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyReward : MonoBehaviour
{
    public Sprite spriteIcon = null;
    public Sprite spriteCollectedReward = null;

    public virtual void GetReward()
    {
        Debug.Log("Base Effect");
    }
}
