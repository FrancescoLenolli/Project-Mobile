using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShipsManager : MonoBehaviour
{
    private List<ShipData> shipsData = new List<ShipData>();

    private void Start()
    {
        shipsData = Resources.LoadAll<ShipData>("Ships").ToList();
    }
}
