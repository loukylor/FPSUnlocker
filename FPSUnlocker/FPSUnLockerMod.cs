using MelonLoader;
using UnityEngine;

[assembly: MelonInfo(typeof(FPSUnlocker.FPSUnLockerMod), "FPSUnlocker", "1.0.0", "loukylor", "https://github.com/loukylor/VRC-Mods")]
[assembly: MelonGame()]

namespace FPSUnlocker
{
    public class FPSUnLockerMod : MelonMod
    {
        public static int fps;
        public static bool vSync;

        public override void OnApplicationStart()
        {
            MelonPrefs.RegisterCategory("FPSUnlocker Config", "FPSUnlocker Config");
            MelonPrefs.RegisterInt("FPSUnlocker Config", "maxFps", 60, "Max FPS");
            MelonPrefs.RegisterBool("FPSUnlocker Config", "vSync", false, "Enable/Disable vSync");
            OnModSettingsApplied();
        }
        public override void OnLevelWasLoaded(int level) => SetValues();
        public override void OnModSettingsApplied()
        {
            fps = MelonPrefs.GetInt("FPSUnlocker Config", "maxFps");
            vSync = MelonPrefs.GetBool("FPSUnlocker Config", "vSync");
            SetValues(true);
        }

        public static void SetValues(bool shouldLog = false)
        {
            if (vSync)
            {
                if (shouldLog) MelonLogger.Log("vSync enabled. Max FPS will be ignored.");
                
                if (QualitySettings.vSyncCount != 1) // Skip if value won't change
                    QualitySettings.vSyncCount = 1;
            }
            else
            {
                if (QualitySettings.vSyncCount != 0)
                    QualitySettings.vSyncCount = 0;
                
                if (shouldLog) MelonLogger.Log("vSync: Off");

                if (fps < 1)
                {
                    if (shouldLog) MelonLogger.LogWarning("FPS attempted to be set to less than 1: resetting to default value of 60.");
                    fps = 60;
                }

                Application.targetFrameRate = fps;
                if (shouldLog) MelonLogger.Log($"FPS set to: {fps}");
            }
        }
    }
}
