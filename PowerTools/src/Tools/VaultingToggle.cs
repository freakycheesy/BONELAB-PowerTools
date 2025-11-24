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

        public override Color ToolTheme => Color.magenta;

        public override bool HaveEnableToggle => true;

        public static VaultingToggle instance;
        public override void Start() {
            base.Start();
        }
        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            instance = this;
        }
        [HarmonyPatch(typeof(PhysicsRig))]
        public static class VaultPatch {
            [HarmonyPatch(nameof(PhysicsRig.CheckDangle)), HarmonyPrefix]
            public static bool CheckDangle(PhysicsRig __instance, ref bool __result)
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
