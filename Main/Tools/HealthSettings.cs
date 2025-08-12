using BoneLib;
using BoneLib.BoneMenu;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Combat;
using MelonLoader;
using System;
using UnityEngine;

namespace PowerTools.Tools {
    public class HealthSettings : BaseTool {
        public static MelonPreferences_Entry<bool> GodMode;
        public static MelonPreferences_Entry<bool> RagdollOnDeath;
        public static MelonPreferences_Entry<bool> ReloadLevel;
        public static MelonPreferences_Entry<float> DeathTime;
        public override void Start() {
            base.Start();
        }
        private static void Hooking_OnPlayerDamageRecieved(float obj) {
            MelonLogger.Msg("Damage Recieved");
            if (GodMode.Value) PlayerHealth.SetFullHealth();
            if (RagdollOnDeath.Value && PlayerHealth.curr_Health <= 0)
                Ragdoll();
        }

        private static void Ragdoll() {
            Player.PhysicsRig.ShutdownRig();
            Player.PhysicsRig.RagdollRig();
        }

        private static void Unragdoll() {
            Player.PhysicsRig.TurnOnRig();
            Player.PhysicsRig.UnRagdollRig();
        }

        public override void MelonCreator() {
            base.MelonCreator();
            GodMode = Main.Preferences.CreateEntry("God Mode", false);
            RagdollOnDeath = Main.Preferences.CreateEntry("Ragdoll On Death", false);
            ReloadLevel = Main.Preferences.CreateEntry("ReloadLevel", false);
            DeathTime = Main.Preferences.CreateEntry("Damage Threshold", 3f);
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page.CreateBool("God Mode", Color.green, GodMode.Value, (a) => {
                GodMode.Value = a;
                Main.Save();
            });
            Page.CreateBool("Ragdoll On Death", Color.green, RagdollOnDeath.Value, (a) => {
                RagdollOnDeath.Value = a;
                PlayerHealth._testRagdollOnDeath = RagdollOnDeath.Value;
                Main.Save();
            });
            Page.CreateBool("Reload Level On Death", Color.green, ReloadLevel.Value, (a) => {
                ReloadLevel.Value = a;
                PlayerHealth.reloadLevelOnDeath = ReloadLevel.Value;
                Main.Save();
                });
            Page.CreateFloat("Death Time", Color.green, DeathTime.Value, 10f, 0f, 100f, (dt) => {
                DeathTime.Value = dt;
                PlayerHealth.deathTimeAmount = DeathTime.Value;
                Main.Save();
            });
            Page.CreateFunction("Die", Color.green, OnDie);
            Page.CreateFunction("Refill Health", Color.green, SetFullHealth);
        }

        public static void SetFullHealth() {
            PlayerHealth?.SetFullHealth();
        }

        [HarmonyPatch(typeof(Player_Health))]
        public static class PlayerHealthPatch {
            [HarmonyPatch(nameof(Health.SetFullHealth)), HarmonyPrefix]
            public static void Respawn() {
                MelonLogger.Msg("Respawn");
                Unragdoll();
            }

            [HarmonyPatch(nameof(Health.Death)), HarmonyPostfix]
            public static void Death() {
                MelonLogger.Msg("Death");
                if (RagdollOnDeath.Value)
                    Ragdoll();
            }

            [HarmonyPatch(nameof(Player_Health.TAKEDAMAGE)), HarmonyPostfix]
            public static void TAKEDAMAGE(float damage) {
                Hooking_OnPlayerDamageRecieved(damage);
            }
        }

        public static Player_Health PlayerHealth {
            get {
                if (Player.RigManager?.health != null)
                    return Player.RigManager.health.TryCast<Player_Health>();
                else
                    return null;
            }
        }

        public override string ToolName => "Health Settings";

        private static void OnDie() {
            PlayerHealth?.Dying(100);
            PlayerHealth?.Death();
            PlayerHealth?.Respawn();
        }

        public override void Reset() {
            base.Reset();
            PlayerHealth.reloadLevelOnDeath = ReloadLevel.Value;
            PlayerHealth.deathTimeAmount = DeathTime.Value;
        }
    }
}
