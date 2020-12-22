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
    private TextMeshProUGUI textButtonCollect = null;

    [HideInInspector] public List<DailyReward> listCurrentRewards = new List<DailyReward>();

    // How many rewards are available.
    // The player can collect one reward every 24 hour.
    public int rewardsNumber = 1;
    public Transform containerRewards = null;
    public GameObject prefabImage = null;
    public Button buttonCollect = null;
    public TextMeshProUGUI textCooldown = null;
    public TextMeshProUGUI textCooldownTime = null;
    [Space(10)]
    [Header("Animation variables")]
    public Transform panelRewards = null;
    public Transform newPosition = null;
    public float animationTime = 1.0f;

    // If all rewards have been collected, reload another list.
    private void LoadRewards()
    {
        // If every reward has been collected, reload Rewards List...
        if (currentRewardIndex == listRewardsIndexes.Count)
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
    }

    private void StartCooldown(long time)
    {
        StartCoroutine(CooldownCollect(time));
    }

    // Display time before next reward is available on the main HUD.
    private void DisplayCooldownTime(TimeSpan timeSpan)
    {
        textCooldownTime.text = timeSpan.TotalSeconds > 0 ? timeSpan.ToString() : "Collect Reward";
    }

    // Change behaviour of collectButton if the Player can collect or not the daily reward.
    private void ChangeButtonBehaviour()
    {
        if (canCollect)
        {
            buttonCollect.onClick.RemoveAllListeners();
            buttonCollect.onClick.AddListener(CollectReward);
            textButtonCollect.text = "COLLECT";
            Debug.Log("Can now collect reward.");
        }
        else
        {
            buttonCollect.onClick.RemoveAllListeners();
            buttonCollect.onClick.AddListener(MoveToPosition);
            textButtonCollect.text = "CLOSE";
            Debug.Log("Cannot collect reward yet.");
        }
    }

    private void LoadData()
    {
        currentRewardIndex = gameManager.playerData.currentRewardIndex;
        listRewardsIndexes = gameManager.playerData.listRewardsIndexes;
        collectionCooldownTime = gameManager.playerData.collectionCooldownTime;

        if (listRewardsIndexes == null)
            listRewardsIndexes = new List<int>();
    }

    // TODO: Link this method to an event in GameManager called when starting the game.
    public void InitData()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        listRewardTypes = new List<DailyReward>(Resources.LoadAll<DailyReward>("DailyRewards"));

        originalPosition = panelRewards.localPosition;
        textButtonCollect = buttonCollect.gameObject.GetComponentInChildren<TextMeshProUGUI>();

        LoadData();
        collectionCooldownTime -= gameManager.GetOfflineTime();
        if (collectionCooldownTime < 0)
            collectionCooldownTime = 0;
        canCollect = collectionCooldownTime > 0 ? false : true;

        StartCooldown(collectionCooldownTime);
        LoadRewards();

    }

    public void CollectReward()
    {
        canCollect = false;
        currentReward.GetReward();
        listRewardsImage[currentRewardIndex].sprite = currentReward.spriteCollectedReward;
        ++currentRewardIndex;
        TimeSpan timeSpan = TimeSpan.FromDays(1);
        StartCooldown((long)timeSpan.TotalSeconds);
        DisplayCooldownTime(timeSpan);
        MoveToPosition();

        Debug.Log("Collecting Reward");
    }

    // Move Panel to the correct spot and change behaviour of the collectButton if needed.
    public void MoveToPosition()
    {
        // Check if the panel is already on the newPosition.
        // Use this value to decide where to move next.
        bool isPanelVisible = panelRewards.localPosition == newPosition.localPosition;

        // If the panel is moving on screen, see what actions you can do with the Collect Button.
        if (!isPanelVisible)
        {
            ChangeButtonBehaviour();
        }

        Vector3 targetPosition = isPanelVisible ? originalPosition : newPosition.localPosition;
        UIManager.Fade fadeType = isPanelVisible ? UIManager.Fade.Out : UIManager.Fade.In;

        uiManager.MoveRectObjectAndFade(animationTime, panelRewards, targetPosition, fadeType);
    }

    public void SaveData()
    {
        gameManager.playerData.listRewardsIndexes = listRewardsIndexes;
        gameManager.playerData.collectionCooldownTime = collectionCooldownTime;
        gameManager.playerData.currentRewardIndex = currentRewardIndex;
    }

    // Once every n time (standard is 24h) the Player can collect a reward.
    private System.Collections.IEnumerator CooldownCollect(long time)
    {
        TimeSpan timeSpan;

        // While time is still passing, display remaining time...
        while (time > 0)
        {
            --time;
            collectionCooldownTime = time;
            timeSpan = TimeSpan.FromSeconds(time);
            textCooldown.text = $"Can collect next reward in:\n{timeSpan}";
            DisplayCooldownTime(timeSpan);
            yield return new WaitForSeconds(1.0f);
        }

        //... when n time has passed, the player can now collect the next reward.
        canCollect = true;
        textCooldown.text = "Collect your reward!";
        DisplayCooldownTime(timeSpan);
    }
}
