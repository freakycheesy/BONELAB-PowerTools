using BoneLib;
using BoneLib.BoneMenu;
using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools {
    public class DeathSettings : BaseTool {
        public static MelonPreferences_Entry<bool> ReloadLevel;
        public static MelonPreferences_Entry<float> DeathTime;

        public override void MelonCreator() {
            base.MelonCreator();
            ReloadLevel = Main.Preferences.CreateEntry("ReloadLevel", false);
            DeathTime = Main.Preferences.CreateEntry("Damage Threshold", 3f);
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page = Main.Player.CreatePage("Death Settings", Color.green);
            CreateEnabledBool(Page, this);
            Page.CreateBool("Reload Level On Death", Color.green, ReloadLevel.Value, (a) => PlayerHealth.reloadLevelOnDeath = a);
            Page.CreateFunction("Die", Color.green, OnDie);
            Page.CreateFloat("Death Time", Color.green, DeathTime.Value, 10f, 0f, 100f, (dt) => {
                DeathTime.Value = dt;
                PlayerHealth.deathTimeAmount = DeathTime.Value;
            });
        }

        public static Player_Health PlayerHealth {
            get {
                if (Player.RigManager?.health != null)
                    return Player.RigManager.health.TryCast<Player_Health>();
                else
                    return null;
            }
        }
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
