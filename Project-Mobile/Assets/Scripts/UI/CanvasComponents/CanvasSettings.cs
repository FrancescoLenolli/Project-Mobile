using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : MonoBehaviour
{
    public Action<bool> EventToggleVolumeSFX;
    public Action<bool> EventToggleVolumeMusic;
    public Action<bool> EventToggleVibration;
    public Action<bool> EventTogglePerformanceMode;

    private bool isVolumeSFXOn;
    private bool isVolumeMusicOn;
    private bool isVibrationOn;
    private bool isPerformanceModeOn;

    [SerializeField] private List<Sprite> listSpritesToggle = new List<Sprite>();
    [SerializeField] private Image imageSFX = null;
    [SerializeField] private Image imageMusic = null;
    [SerializeField] private Image imageVibration = null;
    [SerializeField] private Image imagePerfomanceMode = null;
    [SerializeField] private PanelAnimator panelAnimator = null;

    public void InitData()
    {
        SoundManager musicManager = FindObjectOfType<SoundManager>();

        List<Action<bool>> actionsSFX = new List<Action<bool>> { StatusSFX, Settings.SetVolumeSFX, musicManager.SetSFXVolume};
        List<Action<bool>> actionsMusic = new List<Action<bool>> { StatusMusic, Settings.SetVolumeMusic, musicManager.SetMusicVolume};
        List<Action<bool>> actionsVibration = new List<Action<bool>> { StatusVibration, Settings.SetVibration};
        List<Action<bool>> actionsPerformance = new List<Action<bool>> { StatusPerformanceMode, Settings.SetPerformanceMode};

        Observer.AddObservers(ref EventToggleVolumeSFX, actionsSFX);
        Observer.AddObservers(ref EventToggleVolumeMusic, actionsMusic);
        Observer.AddObservers(ref EventToggleVibration, actionsVibration);
        Observer.AddObservers(ref EventTogglePerformanceMode, actionsPerformance);

        isVolumeSFXOn = Settings.IsVolumeSFXOn;
        isVolumeMusicOn = Settings.IsVolumeMusicOn;
        isVibrationOn = Settings.IsVibrationOn;
        isPerformanceModeOn = Settings.IsPerformanceModeOn;

        imageSFX.sprite = ChangeSprite(isVolumeSFXOn);
        imageMusic.sprite = ChangeSprite(isVolumeMusicOn);
        imageVibration.sprite = ChangeSprite(isVibrationOn);
        imagePerfomanceMode.sprite = ChangeSprite(isPerformanceModeOn);
    }

    public void MoveToPosition()
    {
        panelAnimator.MoveToPosition();
    }

    public void ToggleSFX()
    {
        isVolumeSFXOn = !isVolumeSFXOn;
        EventToggleVolumeSFX?.Invoke(isVolumeSFXOn);
    }

    public void ToggleMusic()
    {
        isVolumeMusicOn = !isVolumeMusicOn;
        EventToggleVolumeMusic(isVolumeMusicOn);
    }

    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        EventToggleVibration?.Invoke(isVibrationOn);
    }

    public void TogglePerformanceMode()
    {
        isPerformanceModeOn = !isPerformanceModeOn;
        EventTogglePerformanceMode?.Invoke(isPerformanceModeOn);
    }

    private void StatusSFX(bool isOn)
    {
        imageSFX.sprite = ChangeSprite(isOn);
    }

    private void StatusMusic(bool isOn)
    {
        imageMusic.sprite = ChangeSprite(isOn);
    }

    private void StatusVibration(bool isOn)
    {
        imageVibration.sprite = ChangeSprite(isOn);
    }

    private void StatusPerformanceMode(bool isOn)
    {
        imagePerfomanceMode.sprite = ChangeSprite(isOn);
    }

    // Change sprite of the button. Not using a Toggle to be more flexible.
    private Sprite ChangeSprite(bool isOn)
    {
        Sprite newSprite = isOn ? listSpritesToggle[1] : listSpritesToggle[0];

        return newSprite;
    }
}
