using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ButtonSound : MonoBehaviour
{
    [EventRef]
    [SerializeField] protected string defaultSound = null;

    protected StudioEventEmitter defaultEmitter = null;

    protected void Awake()
    {
        defaultEmitter = gameObject.AddComponent<StudioEventEmitter>();
        defaultEmitter.Event = defaultSound;
    }

    public void PlaySoundDefault()
    {
        defaultEmitter.Play();
    }
}
