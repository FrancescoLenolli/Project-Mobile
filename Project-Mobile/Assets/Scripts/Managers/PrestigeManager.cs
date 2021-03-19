using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrestigeManager : Singleton<PrestigeManager>
{
    private Action EventReloadingGame;

    public int prestigeLevel;
    public int baseWeight;
    [HideInInspector] public int requiredWeight;

    private new void Awake()
    {
        base.Awake();
    }

    public void InitData()
    {
        SubscribeToEventReloadingGame(GameManager.Instance.Save);

        prestigeLevel = SaveManager.PlayerData.prestigeLevel;
        CalculateRequiredWeight();
    }

    public void SaveData()
    {
        SaveManager.PlayerData.prestigeLevel = prestigeLevel;
    }

    public void PrestigeUp()
    {

        if (GetCollectiblesWeight() >= requiredWeight)
        {
            int premiumReward = CurrencyManager.Instance.data.extrasPremiumCost * 3;

            PlayerData newData = new PlayerData
            {
                prestigeLevel = ++SaveManager.PlayerData.prestigeLevel,
                premiumCurrency = SaveManager.PlayerData.premiumCurrency + premiumReward
            };

            EventReloadingGame?.Invoke();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
        }
    }

    public int GetCollectiblesWeight()
    {
        List<Collectible> collectibles = CurrencyManager.Instance.Collectibles;

        return collectibles.Sum(collectible => (collectible.Weight * collectible.Quantity));
    }

    private void CalculateRequiredWeight()
    {
        requiredWeight = baseWeight * (prestigeLevel + 1);
    }


    private void SubscribeToEventReloadingGame(Action method)
    {
        EventReloadingGame += method;
    }
}
