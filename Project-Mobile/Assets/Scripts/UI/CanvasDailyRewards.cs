using System;
using System.Collections.Generic;
using TMPro;
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
    private long collectionCooldownTime = 0;
    private DailyReward currentReward = null;
    private bool canCollect = true;
    private Vector3 originalPosition = Vector3.zero;

    [HideInInspector] public List<DailyReward> listCurrentRewards = new List<DailyReward>();

    // How many rewards are available.
    // The player can collect one reward every 24 hour.
    public int rewardsNumber = 1;
    public Transform containerRewards = null;
    public GameObject prefabImage = null;
    public Button buttonCollect = null;
    public TextMeshProUGUI textCooldown = null;
    public Button buttonCooldownTime = null;
    public TextMeshProUGUI textCooldownTime = null;
    [Space(10)]
    [Header("Animation variables")]
    public Transform panelRewards = null;
    public Transform newPosition = null;
    public float animationTime = 1.0f;

    private void Awake()
    {
        originalPosition = panelRewards.localPosition;
    }

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
            for (int i = 0; i < listRewardsIndexes.Count; ++i)
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

        if (!gameManager.IsFirstSession() && collectionCooldownTime <= 0)
            MoveToPosition();
        if (gameManager.IsFirstSession())
            buttonCooldownTime.gameObject.SetActive(false);
    }

    private void StartCooldown(long time)
    {
        StartCoroutine(CooldownCollect(time));
    }

    private void DisplayCooldownTime(TimeSpan timeSpan)
    {
        textCooldownTime.text = timeSpan.TotalSeconds > 0 ? timeSpan.ToString() : "Collect Reward";
    }

    // TODO: Link this method to an event in GameManager called when starting the game.
    public void InitRewards()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        listRewardTypes = new List<DailyReward>(Resources.LoadAll<DailyReward>("DailyRewards"));

        // Get saved Data.
        currentRewardIndex = gameManager.playerData.currentRewardIndex;
        listRewardsIndexes = gameManager.playerData.listRewardsIndexes;
        if (listRewardsIndexes == null)
            listRewardsIndexes = new List<int>();
        collectionCooldownTime = gameManager.playerData.rewardCooldownTime;
        collectionCooldownTime -= gameManager.GetOfflineTime();
        if (collectionCooldownTime < 0)
            collectionCooldownTime = 0;
        canCollect = collectionCooldownTime > 0 ? false : true;

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
            listRewardsImage[currentRewardIndex].sprite = currentReward.spriteCollectedReward;
            ++currentRewardIndex;
            TimeSpan timeSpan = TimeSpan.FromDays(1);
            StartCooldown((long)timeSpan.TotalSeconds);
            DisplayCooldownTime(timeSpan);
            MoveToPosition();
        }
        else
        {
            Debug.Log("Cannot collect Reward yet.");
        }
    }

    public void MoveToPosition()
    {
        // Check if the panel is already on the newPosition.
        // Use this value to decide where to move next.
        bool isPanelVisible = panelRewards.localPosition == newPosition.localPosition;

        Vector3 targetPosition = isPanelVisible ? originalPosition : newPosition.localPosition;
        UIManager.Fade fadeType = isPanelVisible ? UIManager.Fade.Out : UIManager.Fade.In;

        uiManager.MoveRectObjectAndFade(animationTime, panelRewards, targetPosition, fadeType);
    }

    private System.Collections.IEnumerator CooldownCollect(long time)
    {
        TimeSpan timeSpan;
        buttonCollect.interactable = canCollect;
        while (time > 0)
        {
            --time;
            collectionCooldownTime = time;
            timeSpan = TimeSpan.FromSeconds(time);
            textCooldown.text = $"Can collect next reward in:\n{timeSpan}";
            DisplayCooldownTime(timeSpan);
            yield return new WaitForSeconds(1.0f);
        }
        canCollect = true;
        buttonCollect.interactable = canCollect;
        textCooldown.text = "Collect your reward!";
        DisplayCooldownTime(timeSpan);
    }

    // TODO: Ugly, find a better solution.
    private void OnApplicationQuit()
    {
        if(gameManager)
            gameManager.SaveRewardsData(listRewardsIndexes, collectionCooldownTime, currentRewardIndex);
    }

}
