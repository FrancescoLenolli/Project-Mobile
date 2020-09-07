using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasOptions : UIElement
{
    private GameManager gameManager = null;
    private bool isSFXVolumeOn = true;
    private bool isMusicVolumeOn = true;
    private bool isVibrationOn = true;

    public List<Sprite> listSpritesToggle = new List<Sprite>();
    public Image imageSFX = null;
    public Image imageMusic = null;
    public Image imageVibration = null;

    private new void Start()
    {
        base.Start();
        gameManager = GameManager.Instance;

        isSFXVolumeOn = gameManager.isSFXVolumeOn;
        isMusicVolumeOn = gameManager.isMusicVolumeOn;
        isVibrationOn = gameManager.isVibrationOn;

        imageSFX.sprite = ChangeSprite(isSFXVolumeOn);
        imageMusic.sprite = ChangeSprite(isMusicVolumeOn);
        imageVibration.sprite = ChangeSprite(isVibrationOn);
    }

    public void ToggleSFX()
    {
        isSFXVolumeOn = !isSFXVolumeOn;
        gameManager.isSFXVolumeOn = isSFXVolumeOn;
        StatusSFX(isSFXVolumeOn);
    }

    public void ToggleMusic()
    {
        isMusicVolumeOn = !isMusicVolumeOn;
        gameManager.isMusicVolumeOn = isSFXVolumeOn;
        StatusMusic(isMusicVolumeOn);
    }

    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        gameManager.isVibrationOn = isVibrationOn;
    }

    private void StatusSFX(bool isOn)
    {
        // SFXVolume = isOn? 1 : 0;
        imageSFX.sprite = ChangeSprite(isOn);
    }

    private void StatusMusic(bool isOn)
    {
        //MusicVolume = isOn ? 1 : 0;
        imageMusic.sprite = ChangeSprite(isOn);
    }

    private void StatusVibration(bool isOn)
    {
        imageVibration.sprite = ChangeSprite(isOn);
    }

    // Change sprite of the button. Not using a Toggle to be more flexible.
    private Sprite ChangeSprite(bool isOn)
    {
        Sprite newSprite;
        newSprite = isOn ? listSpritesToggle[1] : listSpritesToggle[0];
        return newSprite;
    }
}
