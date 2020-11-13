using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasDailyRewards : MonoBehaviour
{
    private GameManager gameManager = null;
    private UIManager uiManager = null;

    [HideInInspector] public List<DailyReward> listCurrentRewards = new List<DailyReward>();

    public List<DailyReward> listDailyRewards = new List<DailyReward>();
    public int rewardsNumber = 1;
    public Transform containerRewards = null;
    public GameObject prefabImage = null;

    public void InitRewards()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;

        listDailyRewards = new List<DailyReward>(Resources.LoadAll<DailyReward>("DailyRewards"));

        for (int i = 0; i < rewardsNumber; ++i)
        {
            int index = Random.Range(0, listDailyRewards.Count);
            GameObject newImage = Instantiate(prefabImage, containerRewards, false);
            newImage.GetComponent<Image>().sprite = listDailyRewards[index].spriteIcon;
        }
    }

    public void CollectReward()
    {
    }
}
