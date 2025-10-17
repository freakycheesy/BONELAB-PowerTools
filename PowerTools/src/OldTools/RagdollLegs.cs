using BoneLib;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;

namespace PowerTools.OldTools {
    internal static class RagdollLegs
    {
        public static void Start() {
            BoneMenuCreator();
        }
        public static void BoneMenuCreator()
        {
            var ragdollLegs = Main.Player.CreatePage("Ragdoll Legs", Color.green);
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