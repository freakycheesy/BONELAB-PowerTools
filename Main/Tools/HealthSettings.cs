using BoneLib;
using BoneLib.BoneMenu;
using HarmonyLib;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Combat;
using Il2CppSLZ.Marrow.SceneStreaming;
using MelonLoader;
using System;
using UnityEngine;

namespace PowerTools.Tools {
    public enum Bool : byte {
        Default = 0,
        True,
        False,
    }
    public class HealthSettings : BaseTool {
        public static MelonPreferences_Entry<bool> GodMode;
        public static MelonPreferences_Entry<bool> RagdollOnDeath;
        public static MelonPreferences_Entry<Bool> ReloadLevel;
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
            ReloadLevel = Main.Preferences.CreateEntry("ReloadLevel", Bool.Default);
            DeathTime = Main.Preferences.CreateEntry("Damage Threshold", 3f);
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page.CreateBool("God Mode", ToolTheme, GodMode.Value, (a) => {
                GodMode.Value = a;
                Main.Save();
            });
            Page.CreateBool("Ragdoll On Death", ToolTheme, RagdollOnDeath.Value, (a) => {
                RagdollOnDeath.Value = a;
                PlayerHealth._testRagdollOnDeath = RagdollOnDeath.Value;
                Main.Save();
            });
            Page.CreateEnum("Reload Level On Death", ToolTheme, ReloadLevel.Value, (a) => {
                ReloadLevel.Value = (Bool)a;
                EditReloadOnDeath(a);
                Main.Save();
            });
            Page.CreateFloat("Death Time", ToolTheme, DeathTime.Value, 10f, 0f, 100f, (dt) => {
                DeathTime.Value = dt;
                PlayerHealth.deathTimeAmount = DeathTime.Value;
                Main.Save();
            });
            Page.CreateFunction("Die", ToolTheme, OnDie);
            Page.CreateFunction("Refill Health", ToolTheme, SetFullHealth);
        }

        public static void EditReloadOnDeath(Enum a) {
            switch (a) {
                case Bool.True:
                    PlayerHealth.reloadLevelOnDeath = true;
                    break;
                case Bool.False:
                    PlayerHealth.reloadLevelOnDeath = true;
                    break;
                case Bool.Default:
                    var decorator = UnityEngine.Object.FindObjectOfType<PlayerHealthDecorator>();
                    if (decorator)
                        PlayerHealth.reloadLevelOnDeath = decorator._reloadLevelOnDeath;
                    break;
            }
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

        public override Color ToolTheme => Color.green + Color.yellow;

        private static void OnDie() {
            PlayerHealth?.Dying(100);
            PlayerHealth?.Death();
            PlayerHealth?.Respawn();
        }

        public override void Reset() {
            base.Reset();
            EditReloadOnDeath(ReloadLevel.Value);
            PlayerHealth.deathTimeAmount = DeathTime.Value;
        }
    }
}
