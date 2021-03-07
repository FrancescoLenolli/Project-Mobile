using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDailyRewards : MonoBehaviour
{
    private Action EventCollectReward;

    private UIManager uiManager;
    private DailyRewardsManager rewardsManager;
    private Vector3 originalPosition;
    private Vector3 newPosition;
    private TextMeshProUGUI textButtonGetReward;
    private List<Image> rewardImages = new List<Image>();

    public Transform panelRewards;
    public Transform targetPosition;
    public Transform rewardsContainer = null;
    public Image imagePrefab = null;
    public TextMeshProUGUI textCooldownTime = null;
    public Button buttonGetReward = null;
    public float animationTime = 0;
    public Color colorCollectedReward;

    private void Start()
    {
        uiManager = UIManager.Instance;
        originalPosition = panelRewards.localPosition;
        newPosition = targetPosition.localPosition;
    }

    public void InitRewards(List<DailyReward> rewards, int currentIndex, DailyRewardsManager dailyRewardsManager)
    {
        rewardsManager = dailyRewardsManager;
        textButtonGetReward = buttonGetReward.GetComponentInChildren<TextMeshProUGUI>();

        EventCollectReward += rewardsManager.CollectReward;

        SpawnRewards(rewards);

        rewardImages.GetRange(0, currentIndex - 1).ForEach(image => image.color = colorCollectedReward);
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
        bool isPanelVisible = panelRewards.localPosition == newPosition;

        Vector3 targetPosition = isPanelVisible ? originalPosition : newPosition;
        UIManager.Fade fadeType = isPanelVisible ? UIManager.Fade.Out : UIManager.Fade.In;

        uiManager.MoveRectObjectAndFade(panelRewards, targetPosition, animationTime, fadeType);
    }

    public void CheckCooldown(int cooldownSeconds)
    {
        if (cooldownSeconds <= 0)
        {
            textCooldownTime.text = "";
            textButtonGetReward.text = "Collect";
            MoveToView();
        }
        else
        {
            textCooldownTime.text = Formatter.FormatTime(cooldownSeconds);
            textButtonGetReward.text = "Close";
        }
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

    private void MoveToView()
    {
        bool isPanelVisible = panelRewards.localPosition == newPosition;

        if(!isPanelVisible)
        uiManager.MoveRectObjectAndFade(panelRewards, newPosition, animationTime, UIManager.Fade.In);
    }
}
