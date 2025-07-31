

using BoneLib;
using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    public static class RagdollOnDeath
    {
        public static MelonPreferences_Entry<bool> RagdollOnDeathIsEnabled { get; set; }
        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }
        public static void MelonPreferencesCreator()
        {
            RagdollOnDeathIsEnabled = Main.MelonPrefCategory.CreateEntry("RagdollOnDeathIsEnabled", false);
        }

        public static void BoneMenuCreator()
        {
            Hooking.OnPlayerDeath += Hooking_OnPlayerDeath;
            var ragdollOnDeathCustomizer = Main.Player.CreatePage("Ragdoll On Death", Color.green);

            ragdollOnDeathCustomizer.CreateBool("Mod Toggle", Color.green, RagdollOnDeathIsEnabled.Value, (a) => {
                RagdollOnDeathIsEnabled.Value = a;
                DeathSettings.PlayerHealth._testRagdollOnDeath = RagdollOnDeathIsEnabled.Value;
            });
        }

        private static void Hooking_OnPlayerDeath() {
            if (!RagdollOnDeathIsEnabled.Value)
                return;
            Player.PhysicsRig.RagdollRig();
            Player.PhysicsRig.Invoke(nameof(PhysicsRig.UnRagdollRig), DeathSettings.PlayerHealth.deathTimeAmount);
        }
    }
}