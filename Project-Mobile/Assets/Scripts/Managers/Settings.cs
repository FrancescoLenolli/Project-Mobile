using UnityEngine;

public static class Settings
{
    private static bool isVolumeSFXOn;
    private static bool isVolumeMusicOn;
    private static bool isVibrationOn;
    private static bool isPerformanceModeOn;

    public static bool IsVolumeSFXOn { get => isVolumeSFXOn; }
    public static bool IsVolumeMusicOn { get => isVolumeMusicOn; }
    public static bool IsVibrationOn { get => isVibrationOn; }
    public static bool IsPerformanceModeOn { get => isPerformanceModeOn; }

    public static void InitData()
    {
        isVolumeSFXOn = SaveManager.PlayerData.isVolumeSFXOn;
        isVolumeMusicOn = SaveManager.PlayerData.isVolumeMusicOn;
        isVibrationOn = SaveManager.PlayerData.isVibrationOn;
        isPerformanceModeOn = SaveManager.PlayerData.isPerformanceModeOn;

        SetPerformanceMode(isPerformanceModeOn);

        UnityEngine.Object.FindObjectOfType<CanvasSettings>().InitData();
    }

    public static void SaveData()
    {
        SaveManager.PlayerData.isVolumeSFXOn = isVolumeSFXOn;
        SaveManager.PlayerData.isVolumeMusicOn = isVolumeMusicOn;
        SaveManager.PlayerData.isVibrationOn = isVibrationOn;
        SaveManager.PlayerData.isPerformanceModeOn = isPerformanceModeOn;
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

        if (isVibrationOn)
        {
            Vibration.VibrateSoft();
        }
    }

    public static void SetPerformanceMode(bool isOn)
    {
        isPerformanceModeOn = isOn;

        Application.targetFrameRate = isPerformanceModeOn ? 60 : 30;
    }
}
