using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ShipButtonSound : ButtonSound
{
    [EventRef]
    [SerializeField] protected string shipUnlockedSound = null;

    private StudioEventEmitter shipEmitter = null;

    private new void Awake()
    {
        base.Awake();
        shipEmitter = gameObject.AddComponent<StudioEventEmitter>();
        shipEmitter.Event = shipUnlockedSound;
    }

    public void PlayShipUnlockedSound(ShipData data)
    {
        shipEmitter.Play();
    }
}
