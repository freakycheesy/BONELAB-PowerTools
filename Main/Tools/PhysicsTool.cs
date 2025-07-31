using HarmonyLib;
using Il2CppSLZ.Marrow;
using MelonLoader;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace PowerTools.Tools {
    public class PhysicsTool : BaseTool {
        public static MelonPreferences_Entry<bool> ForcePullAnything;
        public new static void Start() {
            Reset();
            MelonCreator();
            BoneMenuCreator();
        }

        public new static void MelonCreator() {
            ForcePullAnything = Main.MelonPrefCategory.CreateEntry("ForcePullAnything", false);
        }

        public new static void BoneMenuCreator() {
            var page = Main.Game.CreatePage("Physics Tool", Color.green);
            page.CreateBool("Force Pull Anything", Color.green, ForcePullAnything.Value, ForcePullEveryGrabbable);
        }

        private static void ForcePullEveryGrabbable(bool obj) {
            if (obj) {
                
            }
        }

        public class ForcePullEntity : ForcePullGrip {
            public void Awake() {
                _grip = GetComponent<Grip>();
                maxForce = int.MaxValue;
                maxSpeed = int.MaxValue;
            }
        }

        [HarmonyPatch(typeof(ForcePullEntity))]
        public static class GripPatch {
            public static List<Grip> grips = new List<Grip>();
            [HarmonyPatch(nameof(Grip.Awake))]
            public static void Awake(Grip __instance) {
                grips.Add(__instance);
            }
        }

        public new static void Reset() {
        }
    }
}
