﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class DailyRewardsManager : MonoBehaviour
{
    private Action<int> EventSendCooldownTime;
    private Action<int> EventRewardCollected;
    private Action<List<DailyReward>> EventSendRewards;

    private const int rewardCooldownSeconds = 86400; // seconds in a day;
    private List<DailyReward> rewards = new List<DailyReward>();
    private List<int> rewardsIndexes = new List<int>();
    public int currentIndex = 0;

    public int currentCooldownSeconds = 0;
    public int rewardsCount;

    public void InitData()
    {
        rewardsIndexes = SaveManager.PlayerData.listRewardsIndexes;
        currentIndex = SaveManager.PlayerData.currentRewardIndex;
        currentCooldownSeconds = SaveManager.PlayerData.cooldownSeconds;

        if (rewardsIndexes.Count == 0 || rewardsIndexes.Count == currentIndex)
        {
            SetRewards();
        }
        else
        {
            rewardsIndexes.ForEach(index => rewards.Add(GetReward(index)));
        }

        CanvasDailyRewards canvasDailyRewards = FindObjectOfType<CanvasDailyRewards>();
        canvasDailyRewards.InitRewards(rewards, currentIndex, this);

        SubscribeToEventSendCooldownTime(canvasDailyRewards.CheckCooldown);
        SubscribeToEventSendRewards(canvasDailyRewards.ResetRewards);
        SubscribeToEventRewardCollected(canvasDailyRewards.RewardCollected);

        StartCoroutine(RewardsCooldown());
    }

    public void SaveData()
    {
        SaveManager.PlayerData.listRewardsIndexes = rewardsIndexes;
        SaveManager.PlayerData.currentRewardIndex = currentIndex;
        SaveManager.PlayerData.cooldownSeconds = currentCooldownSeconds;
    }

    public void CollectReward()
    {
        bool canCollect = currentIndex < rewards.Count && currentCooldownSeconds == 0;
        if (canCollect)
        {
            rewards[currentIndex].GetReward();
            EventRewardCollected?.Invoke(currentIndex);
            ++currentIndex;
            ResetCooldown();

            if (currentIndex >= rewards.Count)
            {
                SetRewards();
                EventSendRewards?.Invoke(rewards);
            }
        }
    }

    public void CalculateOfflineTime(TimeSpan timeOffline)
    {
        currentCooldownSeconds -= (int)timeOffline.TotalSeconds;

        if (currentCooldownSeconds < 0)
            currentCooldownSeconds = 0;
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
        currentCooldownSeconds = rewardCooldownSeconds;
    }


    private void SubscribeToEventSendCooldownTime(Action<int> method)
    {
        EventSendCooldownTime += method;
    }

    private void SubscribeToEventSendRewards(Action<List<DailyReward>> method)
    {
        EventSendRewards += method;
    }

    private void SubscribeToEventRewardCollected(Action<int> method)
    {
        EventRewardCollected += method;
    }


    private IEnumerator RewardsCooldown()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            currentCooldownSeconds = currentCooldownSeconds <= 0 ? 0 : --currentCooldownSeconds;
            EventSendCooldownTime?.Invoke(currentCooldownSeconds);

            yield return null;
        }
    }

}
