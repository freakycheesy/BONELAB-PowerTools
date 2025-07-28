using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    public static class ReloadOnDeathCustomizer
    {
        private static MelonPreferences_Entry<bool> MelonPrefEnabled { get; set; }
        private static bool ReloadOnDeathCustomizerIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefReloadLevelOnDeath { get; set; }
        public static bool ReloadLevel;
        public static bool IsDefaultSet { get; set; }
        private static bool _defaultReloadOnDeathSettingValue;

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("ReloadOnDeathCustomizerIsEnabled", false);
            MelonPrefReloadLevelOnDeath = Main.MelonPrefCategory.CreateEntry("Reload Level On Death", false);
            if (MelonPrefEnabled != null)
            {
                ReloadOnDeathCustomizerIsEnabled = MelonPrefEnabled.Value;
            }

            if (MelonPrefReloadLevelOnDeath != null)
            {
                ReloadLevel = MelonPrefReloadLevelOnDeath.Value;
            }
        }



        public static void BoneMenuCreator()
        {
            var reloadOnDeathCustomizer = Main.Category.CreatePage("Reload On Death Customizer", Color.green);

            reloadOnDeathCustomizer.CreateBool("Mod Toggle", Color.green, ReloadOnDeathCustomizerIsEnabled, OnSetEnabled);
            reloadOnDeathCustomizer.CreateBool("Reload Level On Death", Color.green, ReloadLevel, ReloadOnDeathSetter);
        }

        public static void ReloadOnDeathSetter(bool value)
        {
            if (!IsDefaultSet)
            {
                _defaultReloadOnDeathSettingValue = (BoneLib.Player.RigManager.health as Player_Health).reloadLevelOnDeath;
                IsDefaultSet = true;
            }
            if (BoneLib.Player.RigManager != null && ReloadOnDeathCustomizerIsEnabled)
            {
                (BoneLib.Player.RigManager.health as Player_Health).reloadLevelOnDeath = value;
            }
            MelonPrefReloadLevelOnDeath.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }

        private static void OnSetEnabled(bool value)
        {
            ReloadOnDeathCustomizerIsEnabled = value;
            if (!value)
            {
                (BoneLib.Player.RigManager.health as Player_Health).reloadLevelOnDeath = _defaultReloadOnDeathSettingValue;
            }
            else
            {
                ReloadOnDeathSetter(ReloadLevel);
            }
            
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}