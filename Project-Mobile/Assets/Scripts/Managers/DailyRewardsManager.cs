using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DailyRewardsManager : MonoBehaviour
{
    private Action<int> EventSendCooldownTime;
    private Action<List<DailyReward>> EventSendRewards;

    private List<DailyReward> rewards = new List<DailyReward>();
    private List<int> rewardsIndexes = new List<int>();
    private int currentIndex = 0;
    private int cooldownSeconds = 0;

    public int rewardsCount;

    public int CooldownSeconds { get => cooldownSeconds; set => cooldownSeconds = value; }

    public void InitData()
    {
        rewardsIndexes = SaveManager.PlayerData.listRewardsIndexes;
        currentIndex = SaveManager.PlayerData.currentRewardIndex;
        cooldownSeconds = SaveManager.PlayerData.cooldownSeconds;

        if (rewardsIndexes.Count == 0 || rewardsIndexes.Count == currentIndex)
        {
            SetRewards();
        }
        else
        {
            rewardsIndexes.ForEach(index => rewards.Add(GetReward(index)));
        }

        CanvasDailyRewards canvasDailyRewards = FindObjectOfType<CanvasDailyRewards>();
        canvasDailyRewards.InitRewards(rewards, this);

        SubscribeToEventSendCooldownTime(canvasDailyRewards.CheckCooldown);
        SubscribeToEventSendRewards(canvasDailyRewards.SpawnRewards);

        StartCoroutine(RewardsCooldown());
    }

    public void SaveData()
    {
        SaveManager.PlayerData.listRewardsIndexes = rewardsIndexes;
        SaveManager.PlayerData.currentRewardIndex = currentIndex;
        SaveManager.PlayerData.cooldownSeconds = cooldownSeconds;
    }

    public bool CollectReward()
    {
        if (currentIndex < rewards.Count)
        {
            rewards[currentIndex].GetReward();
            ResetCooldown();
            ++currentIndex;
            return true;
        }
        else
        {
            return false;
        }
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

        foreach (Type type in Assembly.GetAssembly(typeof(DailyReward)).GetTypes().Where
            (myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DailyReward))))
        {
            ++count;
        }

        return count;
    }

    private void SetRewards()
    {
        rewards.Clear();
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

    private void ResetCooldown()
    {
        cooldownSeconds = (int)TimeSpan.FromDays(1).TotalSeconds;
    }


    private void SubscribeToEventSendCooldownTime(Action<int> method)
    {
        EventSendCooldownTime += method;
    }

    private void SubscribeToEventSendRewards(Action<List<DailyReward>> method)
    {
        EventSendRewards += method;
    }


    private IEnumerator RewardsCooldown()
    {
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            --cooldownSeconds;
            if(cooldownSeconds == 0)
            {
                SetRewards();
                EventSendRewards?.Invoke(rewards);
            }
            EventSendCooldownTime?.Invoke(cooldownSeconds);
            yield return null;
        }
    }

}
