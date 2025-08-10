using HarmonyLib;
using Il2CppSLZ.Marrow;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerTools.Tools {
    public class VaultingToggle : BaseTool {
        public override string ToolName => "Vaulting Toggle";

        public static VaultingToggle instance;
        public override void Start() {
            base.Start();
            instance = this;
        }
        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
        }
        [HarmonyPatch(typeof(PhysicsRig), "CheckDangle")]
        public static class VaultPatch {
            [HarmonyPrefix]
            public static bool Prefix(PhysicsRig __instance, ref bool __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
            {
                if (!instance.ToolEnabled.Value) {
                    __result = false;
                    return false;
                }
                else {
                    return true;
                }
            }
        }
    }
}
