using BoneLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace PowerTools.Tools
{
    internal static class RagdollLegs
    {
        //need to make a melody patch or whatever so it doesn't rest when unragdolling from something like ragdoll mod
        //what did I mean by this top comment??????????????

        public static MelonPreferences_Entry<bool> VaultingToggleIsEnabled { get; set; }
        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }
        public static void MelonPreferencesCreator()
        {
            VaultingToggleIsEnabled = Main.MelonPrefCategory.CreateEntry("Vaulting Toggle", true);
        }
        public static void BoneMenuCreator()
        {
            var ragdollLegs = Main.Category.CreatePage("Ragdoll Legs", Color.green);
            ragdollLegs.CreateBool("Mod Toggle", Color.green, _isEnabled, OnSetEnabled);

        }
        private static void OnSetEnabled(bool value)
        {
            _isEnabled = value;
            if (value)
            {
                Player.PhysicsRig.PhysicalLegs();
            }
            else
            {
                Player.PhysicsRig.UnRagdollRig();
            }
        }
        private static bool _isEnabled;
    }
}