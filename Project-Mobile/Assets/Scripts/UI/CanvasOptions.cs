using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ChangeVolumeSFX(bool isOn);
public delegate void ChangeVolumeMusic(bool isOn);
public delegate void ChangeVibration(bool isOn);

public class CanvasOptions : MonoBehaviour
{
    public event ChangeVolumeSFX EventChangeVolumeSFX;
    public event ChangeVolumeMusic EventChangeVolumeMusic;
    public event ChangeVibration EventChangeVibration;

    private GameManager gameManager = null;
    private bool isVolumeSFXOn = true;
    private bool isVolumeMusicOn = true;
    private bool isVibrationOn = true;
    private Vector3 originalPosition = Vector3.zero;

    public List<Sprite> listSpritesToggle = new List<Sprite>();
    public Image imageSFX = null;
    public Image imageMusic = null;
    public Image imageVibration = null;
    [Min(0)]
    public Transform panelOptions = null;
    public Transform newPosition = null;
    public float animationTime = 0;

    private void Start()
    {
        gameManager = GameManager.Instance;
        originalPosition = panelOptions.transform.localPosition;

        EventChangeVolumeSFX += StatusSFX;
        EventChangeVolumeSFX += gameManager.SetVolumeSFX;
        EventChangeVolumeMusic += StatusMusic;
        EventChangeVolumeMusic += gameManager.SetVolumeMusic;
        EventChangeVibration += StatusVibration;
        EventChangeVibration += gameManager.SetVibration;

        isVolumeSFXOn = gameManager.isVolumeSFXOn;
        isVolumeMusicOn = gameManager.isVolumeMusicOn;
        isVibrationOn = gameManager.isVibrationOn;

        imageSFX.sprite = ChangeSprite(isVolumeSFXOn);
        imageMusic.sprite = ChangeSprite(isVolumeMusicOn);
        imageVibration.sprite = ChangeSprite(isVibrationOn);
    }

    public void MoveToPosition()
    {
        bool isPanelVisible = panelOptions.localPosition == newPosition.localPosition;

        Vector3 targetPosition = isPanelVisible ? originalPosition : newPosition.localPosition;
        UIManager.Fade fadeType = isPanelVisible ? UIManager.Fade.Out : UIManager.Fade.In;

        //UIManager.Instance.MoveRectObjectAndFade(animationTime, panelOptions, targetPosition, fadeType);
    }

    public void ToggleSFX()
    {
        isVolumeSFXOn = !isVolumeSFXOn;
        EventChangeVolumeSFX?.Invoke(isVolumeSFXOn);
    }

    public void ToggleMusic()
    {
        isVolumeMusicOn = !isVolumeMusicOn;
        EventChangeVolumeMusic(isVolumeMusicOn);
    }

    public void ToggleVibration()
    {
        isVibrationOn = !isVibrationOn;
        EventChangeVibration?.Invoke(isVibrationOn);
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
        // Vibrate when condition is true.
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
