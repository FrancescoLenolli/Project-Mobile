using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDailyRewards : MonoBehaviour
{
    private Action EventCollectReward;

    private DailyRewardsManager rewardsManager;
    private TextMeshProUGUI textButtonGetReward;
    private List<Image> rewardImages = new List<Image>();

    [SerializeField] private Transform rewardsContainer = null;
    [SerializeField] private Image imagePrefab = null;
    [SerializeField] private TextMeshProUGUI textCooldownTime = null;
    [SerializeField] private Button buttonGetReward = null;
    [SerializeField] private PanelAnimator panelAnimator = null;
    [SerializeField] private Color colorCollectedReward = Color.white;

    public void InitRewards(List<DailyReward> rewards, int currentIndex, DailyRewardsManager dailyRewardsManager)
    {
        rewardsManager = dailyRewardsManager;
        textButtonGetReward = buttonGetReward.GetComponentInChildren<TextMeshProUGUI>();

        Observer.AddObserver(ref rewardsManager.EventSendCooldownTime, CheckCooldown);
        Observer.AddObserver(ref rewardsManager.EventSendRewards, ResetRewards);
        Observer.AddObserver(ref rewardsManager.EventRewardCollected, RewardCollected);
        Observer.AddObserver(ref EventCollectReward, rewardsManager.CollectReward);

        SpawnRewards(rewards);
        rewardImages.GetRange(0, currentIndex).ForEach(image => RewardCollected(image));
    }


    public void ResetRewards(List<DailyReward> rewards)
    {
        rewardImages.ForEach(image => Destroy(image.gameObject));
        rewardImages.Clear();
        SpawnRewards(rewards);
    }

    public void CollectReward()
    {
        EventCollectReward?.Invoke();
        MoveToPosition();
    }

    public void MoveToPosition()
    {
        panelAnimator.MoveToPosition();
    }

    public void CheckCooldown(int cooldownSeconds)
    {
        if (cooldownSeconds <= 0)
        {
            textCooldownTime.text = "";
            textButtonGetReward.text = "Collect";
            ShowPanel();
        }
        else
        {
            textCooldownTime.text = Formatter.FormatTime(cooldownSeconds);
            textButtonGetReward.text = "Close";
        }
    }

    public void RewardCollected(int index)
    {
        RewardCollected(rewardImages[index]);
    }

    private void RewardCollected(Image image)
    {
        image.color = colorCollectedReward;
    }

    private void SpawnRewards(List<DailyReward> rewards)
    {
        foreach (DailyReward reward in rewards)
        {
            Image newImage = Instantiate(imagePrefab, rewardsContainer);
            newImage.sprite = reward.GetSprite();
            rewardImages.Add(newImage);
        }
    }

    private void ShowPanel()
    {
        panelAnimator.ShowPanel();
    }
}
