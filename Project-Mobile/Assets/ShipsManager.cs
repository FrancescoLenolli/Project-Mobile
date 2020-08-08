using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    List<ShipData> shipDatas = null;
    List<int> quantities = null; // How much of a shipData is owned 

    private void Start()
    {
        shipDatas = new List<ShipData>(Resources.LoadAll<ShipData>("Ships"));
    }
}
