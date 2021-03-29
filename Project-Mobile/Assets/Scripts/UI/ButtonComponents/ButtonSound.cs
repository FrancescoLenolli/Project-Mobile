using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ButtonSound : MonoBehaviour
{
    protected StudioEventEmitter defaultEmitter = null;
    [EventRef]
    [SerializeField] protected string defaultSound = null;

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
