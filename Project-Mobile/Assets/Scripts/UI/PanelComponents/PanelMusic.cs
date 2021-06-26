using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelMusic : MonoBehaviour
{
    private Action<UtilsUI.Cycle> EventCycleMusic;

    private void Start()
    {
        SoundManager musicManager = FindObjectOfType<SoundManager>();

        Observer.AddObserver(ref EventCycleMusic, musicManager.SwitchTrack);
    }

    public void CycleLeft()
    {
        EventCycleMusic?.Invoke(UtilsUI.Cycle.Left);
    }

    public void CycleRight()
    {
        EventCycleMusic?.Invoke(UtilsUI.Cycle.Right);
    }
}
