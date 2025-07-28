using HarmonyLib;
using Il2CppSLZ.Marrow;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    [HarmonyPatch(typeof(PhysicsRig), "CheckDangle")]
    public class VaultingToggle
    {
        private static bool VaultingToggleIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefVaultingToggle { get; set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefVaultingToggle = Main.MelonPrefCategory.CreateEntry("Vaulting Toggle", true);

            if (MelonPrefVaultingToggle != null)
            {
                VaultingToggleIsEnabled = MelonPrefVaultingToggle.Value;
                OnSetEnabled(VaultingToggleIsEnabled);
            }
        }

        public static void BoneMenuCreator()
        {
            var vaultingToggle = Main.Category.CreatePage("Vaulting Toggle", Color.green); 

            vaultingToggle.CreateBool("Vaulting", Color.green, VaultingToggleIsEnabled, OnSetEnabled);
        }
        
        public static bool Prefix(PhysicsRig __instance, ref bool __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
        {
            if(!VaultingToggleIsEnabled)
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
            VaultingToggleIsEnabled = value;
            MelonPrefVaultingToggle.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
    }
}