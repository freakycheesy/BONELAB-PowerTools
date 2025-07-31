using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.SceneStreaming;
using MelonLoader;
using System;
using UnityEngine;

namespace PowerTools.Tools
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
            ReloadLevel = Main.MelonPrefCategory.CreateEntry("ReloadLevel", false);
            MelonPrefDeathTime = Main.MelonPrefCategory.CreateEntry("Damage Threshold", 3f);
        }

        public static void BoneMenuCreator()
        {
            var deathTimeCustomizer = Main.Player.CreatePage("Death Settings", Color.green);

            deathTimeCustomizer.CreateBool("Reload Level On Death", Color.green, ReloadLevel.Value, (a)=> PlayerHealth.reloadLevelOnDeath = a);
            deathTimeCustomizer.CreateFunction("Die", Color.green, OnDie);
            deathTimeCustomizer.CreateFloat("Death Time", Color.green, MelonPrefDeathTime.Value, 10f, 0f, 100f, (dt) =>
            {
                MelonPrefDeathTime.Value = dt;
                PlayerHealth.deathTimeAmount = MelonPrefDeathTime.Value;
            });
        }

        public static Player_Health PlayerHealth {
            get {
                if (Player.RigManager?.health != null)
                    return Player.RigManager.health.TryCast<Player_Health>();
                else return null;
            }
        }
        private static void OnDie() {
            PlayerHealth.Dying(100);
            PlayerHealth.Death();
            PlayerHealth.Respawn();
        }
    }
}
