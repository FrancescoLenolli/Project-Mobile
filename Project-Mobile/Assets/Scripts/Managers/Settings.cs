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
        isVolumeSFXOn = SaveManager.PlayerData.isVolumeSFXOn;
        isVolumeMusicOn = SaveManager.PlayerData.isVolumeMusicOn;
        isVibrationOn = SaveManager.PlayerData.isVibrationOn;
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
        SaveManager.PlayerData.isVolumeSFXOn = isVolumeSFXOn;
        SaveManager.PlayerData.isVolumeMusicOn = isVolumeMusicOn;
        SaveManager.PlayerData.isVibrationOn = isVibrationOn;
    }
}
