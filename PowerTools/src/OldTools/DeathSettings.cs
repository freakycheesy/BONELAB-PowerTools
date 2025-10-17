using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.SceneStreaming;
using MelonLoader;
using System;
using UnityEngine;

namespace PowerTools.OldTools
{
    internal static class DeathSettings
    {
        public static MelonPreferences_Entry<bool> ReloadLevel {
            get; set;
        }
        public static MelonPreferences_Entry<float> MelonPrefDeathTime { get; set; }

        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }

        public static void MelonPreferencesCreator()
        {
            ReloadLevel = Main.Preferences.CreateEntry("ReloadLevel", false);
            MelonPrefDeathTime = Main.Preferences.CreateEntry("Damage Threshold", 3f);
        }

        public static void BoneMenuCreator() {
            var deathTimeCustomizer = Main.Player.CreatePage("Death Settings", Color.green);

        }
    }
}
