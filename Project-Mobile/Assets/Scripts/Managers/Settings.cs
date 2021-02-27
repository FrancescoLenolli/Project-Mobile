using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    private static bool isVolumeSFXOn;
    private static bool isVolumeMusicOn;
    private static bool isVibrationOn;

    public static bool IsVolumeSFXOn { get => isVolumeSFXOn; }
    public static bool IsVolumeMusicOn { get => isVolumeMusicOn; }
    public static bool IsVibrationOn { get => isVibrationOn; }

    public static void InitData()
    {
        isVolumeSFXOn = SaveManager.GetData().isVolumeSFXOn;
        isVolumeMusicOn = SaveManager.GetData().isVolumeMusicOn;
        isVibrationOn = SaveManager.GetData().isVibrationOn;
    }

    public static void SetVolumeSFX(bool isOn)
    {
        isVolumeSFXOn = isOn;
    }

    public static void SetVolumeMusic(bool isOn)
    {
        isVolumeMusicOn = isOn;
    }

    public static void SetVibration(bool isOn)
    {
        isVibrationOn = isOn;
    }

    public static void SaveData()
    {
        SaveManager.GetData().isVolumeSFXOn = isVolumeSFXOn;
        SaveManager.GetData().isVolumeMusicOn = isVolumeMusicOn;
        SaveManager.GetData().isVibrationOn = isVibrationOn;
    }
}
