using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    public static class ReloadOnDeathCustomizer
    {
        public static MelonPreferences_Entry<bool> ReloadOnDeathCustomizerIsEnabled { get; set; }
        public static MelonPreferences_Entry<bool> ReloadLevel { get; set; }
        public static bool IsDefaultSet { get; set; }
        private static bool _defaultReloadOnDeathSettingValue;
        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }
        public static void MelonPreferencesCreator()
        {
            ReloadOnDeathCustomizerIsEnabled = Main.MelonPrefCategory.CreateEntry("ReloadOnDeathCustomizerIsEnabled", false);
            ReloadLevel = Main.MelonPrefCategory.CreateEntry("Reload Level On Death", false);
        }



        public static void BoneMenuCreator()
        {
            var reloadOnDeathCustomizer = Main.Category.CreatePage("Reload On Death Customizer", Color.green);

            reloadOnDeathCustomizer.CreateBool("Mod Toggle", Color.green, ReloadOnDeathCustomizerIsEnabled.Value, OnSetEnabled);
            reloadOnDeathCustomizer.CreateBool("Reload Level On Death", Color.green, ReloadLevel.Value, ReloadOnDeathSetter);
        }

        public static void ReloadOnDeathSetter(bool value)
        {
            if (!IsDefaultSet)
            {
                _defaultReloadOnDeathSettingValue = (BoneLib.Player.RigManager.health as Player_Health).reloadLevelOnDeath;
                IsDefaultSet = true;
            }
            if (BoneLib.Player.RigManager != null && ReloadOnDeathCustomizerIsEnabled.Value)
            {
                (BoneLib.Player.RigManager.health as Player_Health).reloadLevelOnDeath = value;
            }
            ReloadLevel.Value = value;


        }

        private static void OnSetEnabled(bool value)
        {
            ReloadOnDeathCustomizerIsEnabled.Value = value;
            if (!value)
            {
                (BoneLib.Player.RigManager.health as Player_Health).reloadLevelOnDeath = _defaultReloadOnDeathSettingValue;
            }
            else
            {
                ReloadOnDeathSetter(ReloadLevel.Value);
            }
            
            ReloadOnDeathCustomizerIsEnabled.Value = value;
            MelonPreferences.Save();
        }
    }
}