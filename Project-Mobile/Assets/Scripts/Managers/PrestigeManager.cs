using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PrestigeManager : Singleton<PrestigeManager>, IDataHandler
{
    public static int prestigeLevel;
    public int baseWeight;

    [HideInInspector] public int requiredWeight;

    private CurrencyManager currencyManager;

    private new void Awake()
    {
        base.Awake();
    }

    public void InitData()
    {
        prestigeLevel = SaveManager.PlayerData.prestigeLevel;
        currencyManager = CurrencyManager.Instance;
        CalculateRequiredWeight();
    }

    public void SaveData()
    {
        SaveManager.PlayerData.prestigeLevel = prestigeLevel;
    }

    public void PrestigeUp()
    {
        if (GetCollectiblesWeight() < requiredWeight)
            return;

        int premiumReward = currencyManager.data.extrasPremiumCost * 3;
        ++prestigeLevel;

        PlayerData newData = new PlayerData
        {
            prestigeLevel = prestigeLevel,
            premiumCurrency = SaveManager.PlayerData.premiumCurrency + premiumReward
        };

        SaveManager.Save(newData);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public int GetCollectiblesWeight()
    {
        return currencyManager.Collectibles.Sum(collectible => (collectible.Weight * collectible.Quantity));
    }

    private void CalculateRequiredWeight()
    {
        requiredWeight = baseWeight * (prestigeLevel + 1);
    }
}
