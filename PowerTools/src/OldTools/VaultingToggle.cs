using HarmonyLib;
using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;

namespace PowerTools.OldTools {
    [HarmonyPatch(typeof(PhysicsRig), "CheckDangle")]
    public class VaultingToggle
    {
        public static MelonPreferences_Entry<bool> VaultingToggleIsEnabled { get; set; }
        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }
        public static void MelonPreferencesCreator()
        {
            VaultingToggleIsEnabled = Main.Preferences.CreateEntry("Vaulting Toggle", true);
        }

        public static void BoneMenuCreator()
        {
            var vaultingToggle = Main.Player.CreatePage("Vaulting Toggle", Color.green); 

            vaultingToggle.CreateBool("Vaulting", Color.green, VaultingToggleIsEnabled.Value, OnSetEnabled);
        }
        
        public static bool Prefix(PhysicsRig __instance, ref bool __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
        {
            if(!VaultingToggleIsEnabled.Value)
            {
                __result = false;
                return false;
            }
            else
            {
                return true;
            }
        }

        private static void OnSetEnabled(bool value)
        {
            VaultingToggleIsEnabled.Value = value;

            MelonPreferences.Save();
        }
    }
}