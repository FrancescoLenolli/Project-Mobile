using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : MonoBehaviour
{
    public Action<bool> EventToggleVolumeSFX;
    public Action<bool> EventToggleVolumeMusic;
    public Action<bool> EventToggleVibration;

    private bool isVolumeSFXOn;
    private bool isVolumeMusicOn;
    private bool isVibrationOn;

    [SerializeField] private List<Sprite> listSpritesToggle = new List<Sprite>();
    [SerializeField] private Image imageSFX = null;
    [SerializeField] private Image imageMusic = null;
    [SerializeField] private Image imageVibration = null;
    [SerializeField] private PanelAnimator panelAnimator = null;

    private void Start()
    {
        SubscribeToEventToggleVolumeSFX(StatusSFX);
        SubscribeToEventToggleVolumeSFX(Settings.SetVolumeSFX);
        SubscribeToEventToggleVolumeMusic(StatusMusic);
        SubscribeToEventToggleVolumeMusic(Settings.SetVolumeMusic);
        SubscribeToEventToggleVibration(StatusVibration);
        SubscribeToEventToggleVibration(Settings.SetVibration);

        isVolumeSFXOn = Settings.IsVolumeSFXOn;
        isVolumeMusicOn = Settings.IsVolumeMusicOn;
        isVibrationOn = Settings.IsVibrationOn;

        imageSFX.sprite = ChangeSprite(isVolumeSFXOn);
        imageMusic.sprite = ChangeSprite(isVolumeMusicOn);
        imageVibration.sprite = ChangeSprite(isVibrationOn);
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

        if (isOn)
        {
            Vibration.VibrateSoft();
        }

    }

    // Change sprite of the button. Not using a Toggle to be more flexible.
    private Sprite ChangeSprite(bool isOn)
    {
        Sprite newSprite;
        newSprite = isOn ? listSpritesToggle[1] : listSpritesToggle[0];
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
}
