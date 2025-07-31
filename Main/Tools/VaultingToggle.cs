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
    [HarmonyPatch(typeof(PhysicsRig), "CheckDangle")]
    public class VaultingToggle : BaseTool {
        public static bool enabled => instance.ToolEnabled.Value;
        public static VaultingToggle instance;
        public override void Start() {
            base.Start();
            instance = this;
        }
        public override void BoneMenuCreator() {
            Page = Main.Player.CreatePage("Vaulting Toggle", Color.green);
            CreateEnabledBool(Page, this);
        }

        [HarmonyPrefix]
        public static bool Prefix(PhysicsRig __instance, ref bool __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
        {
            if (!enabled) {
                __result = false;
                return false;
            }
            else {
                return true;
            }
        }
    }
}
