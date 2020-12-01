using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDailyRewards : MonoBehaviour
{
    private GameManager gameManager = null;
    private UIManager uiManager = null;
    private List<DailyReward> listRewardTypes = new List<DailyReward>();
    private List<int> listRewardsIndexes = new List<int>();
    private List<Image> listRewardsImage = new List<Image>();
    private int currentRewardIndex = 0;
    private int collectionCooldownTime = 0;
    private DailyReward currentReward = null;
    private bool canCollect = true;

    [HideInInspector] public List<DailyReward> listCurrentRewards = new List<DailyReward>();

    // How many rewards are available.
    // The player can collect one reward every 24 hour.
    public int rewardsNumber = 1;
    public Transform containerRewards = null;
    public GameObject prefabImage = null;

    // If all rewards have been collected, reload another list.
    private void LoadRewards()
    {
        // If every reward has been collected, reload Rewards List...
        if (currentRewardIndex == listCurrentRewards.Count)
        {
            currentRewardIndex = 0;
            for (int i = 0; i < rewardsNumber; ++i)
            {
                int index = UnityEngine.Random.Range(0, listRewardTypes.Count);

                GameObject newReward = Instantiate(prefabImage, containerRewards, false);
                Image rewardImage = newReward.GetComponent<Image>();
                rewardImage.sprite = listRewardTypes[index].spriteIcon;

                listCurrentRewards.Add(listRewardTypes[index]);
                listRewardsIndexes.Add(index);
                listRewardsImage.Add(rewardImage);
            }
        }

        //...else, used the saved indexes to load the current list.
        else
        {
            for(int i = 0; i < listRewardsIndexes.Count; ++i)
            {
                GameObject newReward = Instantiate(prefabImage, containerRewards, false);
                Image rewardImage = newReward.GetComponent<Image>();

                // Check every reward previously collected with a different sprite.
                rewardImage.sprite =
                    i >= currentRewardIndex ? listRewardTypes[listRewardsIndexes[i]].spriteIcon : listRewardTypes[listRewardsIndexes[i]].spriteCollectedReward;

                listCurrentRewards.Add(listRewardTypes[listRewardsIndexes[i]]);
                listRewardsImage.Add(rewardImage);
            }
        }

        currentReward = listCurrentRewards[currentRewardIndex];
    }

    private void StartCooldown(int time)
    {
        StartCoroutine(CooldownCollect(time));
    }

    // TODO: Link this method to an event in GameManager called when starting the game.
    public void InitRewards()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        listRewardTypes = new List<DailyReward>(Resources.LoadAll<DailyReward>("DailyRewards"));

        listRewardsIndexes = gameManager.playerData.listRewardsIndexes;
        currentRewardIndex = gameManager.playerData.currentRewardIndex;
        collectionCooldownTime = gameManager.playerData.rewardCooldownTime;

        collectionCooldownTime -= gameManager.GetOfflineTime();

        StartCooldown(collectionCooldownTime);
        LoadRewards();

    }

    public void CollectReward()
    {
        // Prevent the Player from collecting the same reward multiple times.
        if (canCollect)
        {
            canCollect = false;
            currentReward.GetReward();
            ++currentRewardIndex;
            StartCooldown((int)TimeSpan.FromDays(1).TotalSeconds);
        }
        else
        {
            Debug.Log("Cannot collect Reward yet.");
        }
    }

    private System.Collections.IEnumerator CooldownCollect(int time)
    {
        while (time > 0)
        {
            --time;
            yield return new WaitForSeconds(1.0f);
        }
        canCollect = true;
    }

    // TODO: Ugly, find a better solution.
    private void OnApplicationQuit()
    {
        gameManager.SaveRewardsData(listRewardsIndexes, collectionCooldownTime, currentRewardIndex);
    }

}
