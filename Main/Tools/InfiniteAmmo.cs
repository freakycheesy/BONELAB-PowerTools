using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Data;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    
    public abstract class InfiniteAmmo // this whole thing is so janky but if it works it works
    {
        
        private static MelonPreferences_Entry<bool> MelonPrefInfiniteAmmo { get; set; }
        private static bool InfiniteAmmoIsEnabled { get; set; }
        
        private static MelonPreferences_Entry<bool> MelonPrefGiveAmmoWhenEmpty { get; set; }

        private static bool GiveAmmoWhenEmpty { get; set; }
        
        private static MelonPreferences_Entry<bool> MelonPrefInfiniteMags { get; set; }

        private static bool InfiniteMags { get; set; }
        
        private static MelonPreferences_Entry<bool> MelonPrefAutoChamber { get; set; }

        private static bool AutoChamber { get; set; }
        
        private static MelonPreferences_Entry<bool> MelonPrefAutoLoad { get; set; }

        private static bool AutoLoad { get; set; }
        
        public static void MelonPreferencesCreator()
        {
            
            MelonPrefInfiniteAmmo = Main.MelonPrefCategory.CreateEntry("InfiniteAmmoIsEnabled", false);
            MelonPrefGiveAmmoWhenEmpty = Main.MelonPrefCategory.CreateEntry("GiveAmmoWhenEmpty", false);
            MelonPrefInfiniteMags = Main.MelonPrefCategory.CreateEntry("InfiniteMags", false);
            MelonPrefAutoChamber = Main.MelonPrefCategory.CreateEntry("AutoChamber", false);
            MelonPrefAutoLoad = Main.MelonPrefCategory.CreateEntry("AutoLoad", false);
            if (MelonPrefInfiniteAmmo != null)
            {
                InfiniteAmmoIsEnabled = MelonPrefInfiniteAmmo.Value;
            }
            if (MelonPrefGiveAmmoWhenEmpty != null)
            {
                GiveAmmoWhenEmpty = MelonPrefGiveAmmoWhenEmpty.Value;
            }
            if (MelonPrefInfiniteMags != null)
            {
                InfiniteMags = MelonPrefInfiniteMags.Value;
            }
            if (MelonPrefAutoChamber != null)
            {
                AutoChamber = MelonPrefAutoChamber.Value;
            }
            if (MelonPrefAutoLoad != null)
            {
                AutoLoad = MelonPrefAutoLoad.Value;
            }
        }
        public static void BoneMenuCreator()
        {
            var infiniteAmmo = Main.Category.CreatePage("Infinite Ammo", Color.green);
            infiniteAmmo.CreateBool("Infinite Ammo", Color.green, InfiniteAmmoIsEnabled, OnSetEnabled);
            infiniteAmmo.CreateBool("Give ammo when mag can't be full", Color.green, GiveAmmoWhenEmpty, OnGiveAmmoWhenEmpty);
            infiniteAmmo.CreateBool("Auto Chamber", Color.green, AutoChamber, OnAutoChamber);
            infiniteAmmo.CreateBool("Auto Load Guns", Color.green, GiveAmmoWhenEmpty, OnGiveAmmoWhenEmpty);
            infiniteAmmo.CreateBool("Infinite Mags", Color.green, InfiniteMags, OnInfiniteMags);
        }

        private static void OnSetEnabled(bool value)
        {
            MelonPrefInfiniteAmmo.Value = value;
            InfiniteAmmoIsEnabled = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        
        private static void OnGiveAmmoWhenEmpty(bool value)
        {
            MelonPrefGiveAmmoWhenEmpty.Value = value;
            GiveAmmoWhenEmpty = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        
        private static void OnInfiniteMags(bool value)
        {
            MelonPrefInfiniteMags.Value = value;
            InfiniteMags = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        
        private static void OnAutoChamber(bool value)
        {
            MelonPrefAutoChamber.Value = value;
            AutoChamber = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        private static void OnAutoLoad(bool value)
        {
            MelonPrefAutoLoad.Value = value;
            AutoLoad = value;
            Main.MelonPrefCategory.SaveToFile(false);
        }
        
        

        [HarmonyPatch(typeof(AmmoInventory), "RemoveCartridge")]
        public class TaxReturn
        {
            private static void Prefix(AmmoInventory __instance, CartridgeData cartridge, int count)
            {
                if (InfiniteAmmoIsEnabled)
                {
                    __instance.AddCartridge(cartridge, count);
                }
            }
        }

        [HarmonyPatch(typeof(InventoryAmmoReceiver), "OnHandGrab")]
        public class AmmoCheckPatch
        {
            public static void Prefix()
            {
                if (InfiniteAmmoIsEnabled && GiveAmmoWhenEmpty)
                {
                    var light = AmmoInventory.Instance.GetCartridgeCount("light");
                    if (light <= 0)
                    {
                        Bankruptcy(AmmoInventory.Instance.lightAmmoGroup);
                    }

                    var medium = AmmoInventory.Instance.GetCartridgeCount("medium");
                    if (medium <= 0)
                    {
                        Bankruptcy(AmmoInventory.Instance.mediumAmmoGroup);
                    }

                    var heavy = AmmoInventory.Instance.GetCartridgeCount("heavy");
                    if (heavy <= 25)
                    {
                        Bankruptcy(AmmoInventory.Instance.heavyAmmoGroup);
                    }
                }
            }
            

            private static void Bankruptcy(AmmoGroup group)
            {
                AmmoInventory.Instance.AddCartridge(group, 1);
            }
        }

        [HarmonyPatch(typeof(Magazine), "OnGrab")]
        public class CashBack
        {
                public static void Postfix()
                {
                    if (InfiniteAmmoIsEnabled && GiveAmmoWhenEmpty)
                    {
                        var leftMag = Player.GetComponentInHand<Magazine>(Player.LeftHand);
                        var rightMag = Player.GetComponentInHand<Magazine>(Player.RightHand);
                        if (leftMag != null)
                        {
                            int leftMagMax = leftMag.magazineState.magazineData.rounds;
                            int leftCartridgeCount = AmmoInventory.Instance.GetCartridgeCount(leftMag.magazineState.cartridgeData);
                            if (leftCartridgeCount < leftMagMax)
                            {
                                BankStatement(leftMag, leftMagMax, leftCartridgeCount);
                            }
                        }

                        if (rightMag != null)
                        {
                            int rightMagMax = rightMag.magazineState.magazineData.rounds;
                            int rightCartridgeCount =
                                AmmoInventory.Instance.GetCartridgeCount(rightMag.magazineState.cartridgeData);
                            if (rightCartridgeCount < rightMagMax)
                            {
                                BankStatement(rightMag, rightMagMax, rightCartridgeCount);
                            }
                        }
                    }
                }
                private static void BankStatement(Magazine mag, int magMax, int cartridgeCount)
                    {
                        if (AmmoInventory.Instance.GetCartridgeCount("light") == cartridgeCount)
                        {
                            Loan(mag, AmmoInventory.Instance.lightAmmoGroup, magMax);
                        }
                        else if (AmmoInventory.Instance.GetCartridgeCount("medium") == cartridgeCount)
                        {
                            Loan(mag, AmmoInventory.Instance.mediumAmmoGroup, magMax);
                        }
                        else if (AmmoInventory.Instance.GetCartridgeCount("heavy") == cartridgeCount)
                        {
                            Loan(mag, AmmoInventory.Instance.heavyAmmoGroup, magMax);
                        }

                    }

                    private static void Loan(Magazine mag, AmmoGroup group, int amount)
                    {
                        if (AmmoInventory.Instance.GetCartridgeCount(group.KeyName) == 1)
                        {
                            AmmoInventory.Instance.AddCartridge(group, -1);
                        }
                        AmmoInventory.Instance.AddCartridge(group, amount);
                        mag.magazineState.Refill();
                    }
        }

        [HarmonyPatch(typeof(Gun), "OnFire")]
        public class CreditCard
        {
            public static void Postfix(Gun __instance)
            {
                if (InfiniteAmmoIsEnabled && InfiniteMags && __instance.gameObject.GetComponentInChildren<Magazine>() != null)
                {
                    __instance.gameObject.GetComponentInChildren<Magazine>().magazineState.Refill();
                }
            }
            
        }

        [HarmonyPatch(typeof(Gun), "AmmoCount")]
        public class ShotgunCreditCard
        {
            public static bool Prefix(Gun __instance, ref int __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
            {
                if (InfiniteAmmoIsEnabled && InfiniteMags)
                {
                    __result = 1;
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        [HarmonyPatch(typeof(Gun), "CheckGunRequirements")]
        public class Paycheck
        {
            public static void Prefix(Gun __instance) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
            {
                if (InfiniteAmmoIsEnabled && AutoChamber)
                {
                    __instance.Charge();
                }
                
                if (InfiniteAmmoIsEnabled && AutoLoad && !__instance._hasMagState)
                {
                    __instance.InstantLoadAsync();
                }
            }
            
        }
        [HarmonyPatch(typeof(Gun), "OnTriggerGripAttached")]
        public class DirectDeposit
        {
            public static void Prefix(Gun __instance) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
            {
                if (InfiniteAmmoIsEnabled && AutoLoad && !__instance._hasMagState)
                {
                    __instance.InstantLoadAsync();
                }
            }
        }
    }
}
