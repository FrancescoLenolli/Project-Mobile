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
        SubscribeToEventToggleVolumeSFX(StatusSFX);
        SubscribeToEventToggleVolumeSFX(Settings.SetVolumeSFX);
        SubscribeToEventToggleVolumeMusic(StatusMusic);
        SubscribeToEventToggleVolumeMusic(Settings.SetVolumeMusic);
        SubscribeToEventToggleVibration(StatusVibration);
        SubscribeToEventToggleVibration(Settings.SetVibration);
        SubscribeToEventTogglePerformanceMode(StatusPerformanceMode);
        SubscribeToEventTogglePerformanceMode(Settings.SetPerformanceMode);

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


    private void SubscribeToEventToggleVolumeSFX(Action<bool> method)
    {
        EventToggleVolumeSFX += method;
    }

    private void SubscribeToEventToggleVolumeMusic(Action<bool> method)
    {
        EventToggleVolumeMusic += method;
    }

    private void SubscribeToEventToggleVibration(Action<bool> method)
    {
        EventToggleVibration += method;
    }

    private void SubscribeToEventTogglePerformanceMode(Action<bool> method)
    {
        EventTogglePerformanceMode += method;
    }
}
