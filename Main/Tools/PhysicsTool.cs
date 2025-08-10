using BoneLib.BoneMenu;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using MelonLoader;
using System.Collections.Generic;
using UnityEngine;

namespace PowerTools.Tools {
    public class PhysicsTool : BaseTool {
        public static MelonPreferences_Entry<bool> ForcePullAnything;
        public static float maxForce = 1000;
        public static float maxSpeed = 1E+10f;

        public override string ToolName => "Physics Tool";

        public override void MelonCreator() {
            base.MelonCreator();
            ForcePullAnything = Main.Preferences.CreateEntry("ForcePullAnything", false);
        }
        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page.CreateBool("Force Pull Anything (Cannot Reverse)", Color.green, ForcePullAnything.Value, (a) => {
                ForcePullAnything.Value = a;
                foreach (var grip in Resources.FindObjectsOfTypeAll<Grip>()) {
                    GripPatch.AddForcePull(grip);
                }
    Main.Save();
            });
        }

        [HarmonyPatch(typeof(Grip))]
        private static class GripPatch {

            [HarmonyPatch(nameof(Grip.Awake))]
            [HarmonyPostfix]
            private static void Awake(Grip __instance) {
                AddForcePull(__instance);
            }

            public static void AddForcePull(Grip __instance) {
                if (!ForcePullAnything.Value)
                    return;
                if (!__instance.gameObject.TryGetComponent(out ForcePullGrip forcePull)) forcePull = __instance.gameObject.AddComponent<ForcePullGrip>();
                forcePull.gameObject.layer = LayerMask.NameToLayer("Interactable");
                forcePull._grip = __instance;
                forcePull.maxForce = maxForce;
                forcePull.maxSpeed = maxSpeed;
            }
        }
    }
}
