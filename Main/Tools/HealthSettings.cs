using BoneLib;
using BoneLib.BoneMenu;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Combat;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools {
    public class HealthSettings : BaseTool {
        public static MelonPreferences_Entry<bool> GodMode;
        public static MelonPreferences_Entry<bool> RagdollOnDeath;
        public static MelonPreferences_Entry<bool> ReloadLevel;
        public static MelonPreferences_Entry<float> DeathTime;

        private static void Hooking_OnPlayerDamageRecieved(float obj) {
            MelonLogger.Msg("Damage Recieved");
            if (GodMode.Value) PlayerHealth.SetFullHealth();
            if (RagdollOnDeath.Value)
                if (PlayerHealth.curr_Health <= 0) {
                    Player.PhysicsRig.ShutdownRig();
                    Player.PhysicsRig.RagdollRig();
                }
        }
        public override void MelonCreator() {
            base.MelonCreator();
            Hooking.OnPlayerDamageRecieved += Hooking_OnPlayerDamageRecieved;
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
        }

        [HarmonyPatch(typeof(Player_Health))]
        public static class PlayerHealthPatch {
            [HarmonyPatch(nameof(Player_Health.Respawn))]
            public static void Respawn() {
                MelonLogger.Msg("Respawn");
                Player.PhysicsRig.TurnOnRig();
                Player.PhysicsRig.UnRagdollRig();
            }
            [HarmonyPatch(nameof(Player_Health.OnReceivedDamage))]
            public static void OnReceivedDamage(Attack attack, PlayerDamageReceiver.BodyPart part) {
                Hooking_OnPlayerDamageRecieved(0);
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
            PlayerHealth.Dying(100);
            PlayerHealth.Death();
            PlayerHealth.Respawn();
        }

        public override void Reset() {
            base.Reset();
            PlayerHealth.reloadLevelOnDeath = ReloadLevel.Value;
            PlayerHealth.deathTimeAmount = DeathTime.Value;
        }
    }
}
