

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
            var ragdollOnDeathCustomizer = Main.Category.CreatePage("Ragdoll On Death", Color.green);

            ragdollOnDeathCustomizer.CreateBool("Mod Toggle", Color.green, RagdollOnDeathIsEnabled.Value, OnSetEnabled);
        }


        public static void OnSetEnabled(bool value)
        {
            BoneLib.Player.RigManager.health._testRagdollOnDeath = value;
            RagdollOnDeathIsEnabled.Value = value;
            RagdollOnDeathIsEnabled.Value = value;


        }
    }
}