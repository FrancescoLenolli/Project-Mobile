using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DailyReward
{
    public virtual void GetReward()
    {
        Debug.Log("Base Effect");
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }
}
