using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : MonoBehaviour
{
    public Action<bool> EventToggleVolumeSFX;
    public Action<bool> EventToggleVolumeMusic;
    public Action<bool> EventToggleVibration;

    private UIManager uiManager;
    private bool isVolumeSFXOn;
    private bool isVolumeMusicOn;
    private bool isVibrationOn;
    private Vector3 originalPosition;
    private Vector3 newPosition;

    [SerializeField] private List<Sprite> listSpritesToggle = new List<Sprite>();
    [SerializeField] private Image imageSFX = null;
    [SerializeField] private Image imageMusic = null;
    [SerializeField] private Image imageVibration = null;
    [SerializeField] private Transform panelOptions = null;
    [SerializeField] private Transform targetPosition = null;
    [SerializeField] private float animationTime = 0;

    private void Start()
    {
        uiManager = UIManager.Instance;
        originalPosition = panelOptions.localPosition;
        newPosition = targetPosition.localPosition;

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
        bool isPanelVisible = panelOptions.localPosition == newPosition;

        Vector3 targetPosition = isPanelVisible ? originalPosition : newPosition;
        UIManager.Fade fadeType = isPanelVisible ? UIManager.Fade.Out : UIManager.Fade.In;

        uiManager.MoveRectObjectAndFade(panelOptions, targetPosition, animationTime, fadeType);
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
}
