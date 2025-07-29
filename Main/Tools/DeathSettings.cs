using Il2CppSLZ.Marrow;
using MelonLoader;
using System;
using UnityEngine;

namespace PowerTools.Tools
{
    internal static class DeathSettings
    {
        public static MelonPreferences_Entry<bool> MelonPrefEnabled { get; set; }
        public static MelonPreferences_Entry<float> MelonPrefDeathTime { get; set; }

        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("DeathTimeCustomizerIsEnabled", false);
            MelonPrefDeathTime = Main.MelonPrefCategory.CreateEntry("Damage Threshold", 3f);
        }

        public static void BoneMenuCreator()
        {
            var deathTimeCustomizer = Main.Category.CreatePage("Death Settings", Color.green);

            deathTimeCustomizer.CreateBool("Mod Toggle", Color.green, MelonPrefEnabled.Value, OnSetEnabled);
            deathTimeCustomizer.CreateFunction("Die", Color.green, OnDie);
            deathTimeCustomizer.CreateFloat("Death Time", Color.green, MelonPrefDeathTime.Value, 1f, 0f, 100f, (dt) =>
            {
                MelonPrefDeathTime.Value = dt;
                MelonPreferences.Save();
                DeathTimeSetter();
            });
        }

        private static void OnDie() {
            BoneLib.Player.RigManager.health.Dying(100);
            BoneLib.Player.RigManager.health.Death();
        }

        public static void DeathTimeSetter()
        {
            if (BoneLib.Player.RigManager != null && MelonPrefEnabled.Value)
            {
                (BoneLib.Player.RigManager.health as Player_Health).deathTimeAmount = MelonPrefDeathTime.Value;
            }
        }

        private static void OnSetEnabled(bool value)
        {
            if (!value)
            {
                (BoneLib.Player.RigManager.health as Player_Health).deathTimeAmount = 3;
            }
            MelonPrefEnabled.Value = value;
            MelonPreferences.Save();
        }
    }
}
