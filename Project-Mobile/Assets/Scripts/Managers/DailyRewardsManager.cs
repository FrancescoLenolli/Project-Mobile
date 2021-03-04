using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DailyRewardsManager : MonoBehaviour
{
    private List<DailyReward> rewards = new List<DailyReward>();
    private List<int> rewardsIndexes = new List<int>();
    private int currentIndex = 0;

    public int rewardsCount;

    public void InitData()
    {
        rewardsIndexes = SaveManager.PlayerData.listRewardsIndexes;
        currentIndex = SaveManager.PlayerData.currentRewardIndex;

        if (rewardsIndexes.Count == 0 || rewardsIndexes.Count == currentIndex)
        {
            rewardsIndexes.Clear();
            currentIndex = 0;

            int totalRewards = GetRewardsCount();
            System.Random random = new System.Random();

            for (int i = 0; i < rewardsCount; ++i)
            {
                int index = random.Next(0, totalRewards);
                rewards.Add(GetReward(index));
                rewardsIndexes.Add(index);
            }
        }
        else
        {
            rewardsIndexes.ForEach(index => rewards.Add(GetReward(index)));
        }

        FindObjectOfType<CanvasDailyRewards>().InitRewards(rewards, this);
    }

    public void SaveData()
    {
        SaveManager.PlayerData.listRewardsIndexes = rewardsIndexes;
        SaveManager.PlayerData.currentRewardIndex = currentIndex;
    }

    public bool CollectReward()
    {
        if (currentIndex < rewards.Count)
        {
            rewards[currentIndex].GetReward();
            ++currentIndex;
            return true;
        }
        return false;
    }

    private DailyReward GetReward(int index)
    {
        DailyReward reward = null;

        switch (index)
        {
            case 0:
                reward = new DRCurrency();
                break;
            case 1:
                reward = new DRPremiumCurrency();
                break;
            case 2:
                reward = new DRDoubleGainTime();
                break;
            default:
                break;
        }

        if (reward == null)
        {
            Debug.LogWarning("Some reward has value NULL");
        }
        return reward;
    }

    private int GetRewardsCount()
    {
        int count = 0;

        // For the memes.
        foreach (Type type in Assembly.GetAssembly(typeof(DailyReward)).GetTypes().Where
            (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DailyReward))))
        {
            ++count;
        }

        return count;
    }
}
