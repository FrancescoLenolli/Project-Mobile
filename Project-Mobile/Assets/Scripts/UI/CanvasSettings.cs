using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : MonoBehaviour
{
    public Action<bool> EventToggleVolumeSFX;
    public Action<bool> EventToggleVolumeMusic;
    public Action<bool> EventToggleVibration;

    private GameManager gameManager;
    private UIManager uiManager;
    private bool isVolumeSFXOn;
    private bool isVolumeMusicOn;
    private bool isVibrationOn;
    private Vector3 originalPosition;
    private Vector3 newPosition;

    public List<Sprite> listSpritesToggle = new List<Sprite>();
    public Image imageSFX = null;
    public Image imageMusic = null;
    public Image imageVibration = null;
    public Transform panelOptions = null;
    public Transform targetPosition = null;
    public float animationTime = 0;

    private void Start()
    {
        gameManager = GameManager.Instance;
        uiManager = UIManager.Instance;
        originalPosition = panelOptions.localPosition;
        newPosition = targetPosition.localPosition;

        SubscribeToEventToggleVolumeSFX(StatusSFX);
        SubscribeToEventToggleVolumeSFX(gameManager.SetVolumeSFX);
        SubscribeToEventToggleVolumeMusic(StatusMusic);
        SubscribeToEventToggleVolumeMusic(gameManager.SetVolumeMusic);
        SubscribeToEventToggleVibration(StatusVibration);
        SubscribeToEventToggleVibration(gameManager.SetVibration);

        isVolumeSFXOn = gameManager.isVolumeSFXOn;
        isVolumeMusicOn = gameManager.isVolumeMusicOn;
        isVibrationOn = gameManager.isVibrationOn;

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
