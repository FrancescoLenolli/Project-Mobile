using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDailyRewards : MonoBehaviour
{
    private Func<bool> FuncCollectReward;

    private UIManager uiManager;
    private DailyRewardsManager rewardsManager;
    private Vector3 originalPosition;
    private Vector3 newPosition;

    public Transform panelRewards;
    public Transform targetPosition;
    public Transform rewardsContainer = null;
    public Image imagePrefab = null;
    public float animationTime = 0;

    private void Start()
    {
        uiManager = UIManager.Instance;
        originalPosition = panelRewards.localPosition;
        newPosition = targetPosition.localPosition;
    }

    public void InitRewards(List<DailyReward> rewards, DailyRewardsManager dailyRewardsManager)
    {
        rewardsManager = dailyRewardsManager;

        FuncCollectReward += rewardsManager.CollectReward;
        foreach(DailyReward reward in rewards)
        {
            Image newImage = Instantiate(imagePrefab, rewardsContainer);
            newImage.sprite = reward.GetSprite();
        }
    }

    public void CollectReward()
    {
        if(CallFuncCollectReward())
        {
            MoveToPosition();
        }
    }

    public void MoveToPosition()
    {
        bool isPanelVisible = panelRewards.localPosition == newPosition;

        Vector3 targetPosition = isPanelVisible ? originalPosition : newPosition;
        UIManager.Fade fadeType = isPanelVisible ? UIManager.Fade.Out : UIManager.Fade.In;

        uiManager.MoveRectObjectAndFade(panelRewards, targetPosition, animationTime, fadeType);
    }

    private bool CallFuncCollectReward()
    {
        if(FuncCollectReward == null)
        {
            return false;
        }
        return FuncCollectReward.Invoke();
    }
}
