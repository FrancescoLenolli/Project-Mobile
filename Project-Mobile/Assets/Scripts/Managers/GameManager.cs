using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public SaveManager saveManager;
    public CurrencyManager currencyManager;
    public ShipsManager shipsManager;
    public InputManager inputManager;

    private void OnApplicationQuit()
    {
        //ShipsManager.Instance.SetQuantities(); // [!!!] This is just to make it work, I'll change it later.
        SaveManager.Instance.SaveCurrentData();
    }
}
