using FMOD.Studio;
using FMODUnity;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [EventRef]
    public List<string> tracks = new List<string>();

    private Bus musicBus;
    private Bus sfxBus;
    private StudioEventEmitter musicEmitter;
    private int trackIndex = 0;

    private void Start()
    {
        musicBus = RuntimeManager.GetBus("bus:/Master/Music");
        sfxBus = RuntimeManager.GetBus("bus:/Master/SFX");

        SetMusicVolume(Settings.IsVolumeMusicOn);
        SetSFXVolume(Settings.IsVolumeSFXOn);

        musicEmitter = gameObject.AddComponent<StudioEventEmitter>();
        musicEmitter.Event = tracks[trackIndex];
        musicEmitter.Play();
    }

    public void SetMusicVolume(bool isOn)
    {
        musicBus.setMute(!isOn);
    }

    public void SetSFXVolume(bool isOn)
    {
        sfxBus.setMute(!isOn);
    }

    public void SwitchTrack(UtilsUI.Cycle cycle)
    {
        trackIndex = UtilsUI.CycleListIndexOpen(trackIndex, tracks.Count, cycle);
        musicEmitter.Stop();
        musicEmitter.Event = tracks[trackIndex];
        musicEmitter.Play();
    }
}
