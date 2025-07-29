using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Data;
using MelonLoader;
using System;
using UnityEngine;

namespace PowerTools.Tools {

    public abstract class InfiniteAmmo // this whole thing is so janky but if it works it works
    {

        public static MelonPreferences_Entry<bool> InfAmmo {
            get; set;
        }

        public static MelonPreferences_Entry<bool> GiveAmmoWhenEmpty {
            get; set;
        }

        public static MelonPreferences_Entry<bool> InfMags {
            get; set;
        }

        public static MelonPreferences_Entry<bool> AutoChamber {
            get; set;
        }

        public static MelonPreferences_Entry<bool> AutoLoad {
            get; set;
        }
        public static MelonPreferences_Entry<bool> MidasTouch {
            get; set;
        }

        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }
        public static void MelonPreferencesCreator() {
            InfAmmo = Main.MelonPrefCategory.CreateEntry("InfiniteAmmoIsEnabled", false);
            GiveAmmoWhenEmpty = Main.MelonPrefCategory.CreateEntry("GiveAmmoWhenEmpty", false);
            InfMags = Main.MelonPrefCategory.CreateEntry("InfiniteMags", false);
            AutoChamber = Main.MelonPrefCategory.CreateEntry("AutoChamber", false);
            AutoLoad = Main.MelonPrefCategory.CreateEntry("AutoLoad", false);
            MidasTouch = Main.MelonPrefCategory.CreateEntry("MidasTouch", false);
        }
        public static void BoneMenuCreator() {
            var infiniteAmmo = Main.Game.CreatePage("Infinite Ammo", Color.green);
            infiniteAmmo.CreateBool("Infinite Ammo", Color.green, InfAmmo.Value, OnSetEnabled);
            infiniteAmmo.CreateBool("Give ammo when mag can't be full", Color.green, GiveAmmoWhenEmpty.Value, OnGiveAmmoWhenEmpty);
            infiniteAmmo.CreateBool("Auto Chamber", Color.green, AutoChamber.Value, OnAutoChamber);
            infiniteAmmo.CreateBool("Auto Load Guns", Color.green, GiveAmmoWhenEmpty.Value, OnGiveAmmoWhenEmpty);
            infiniteAmmo.CreateBool("Infinite Mags", Color.green, InfMags.Value, OnInfiniteMags);
            infiniteAmmo.CreateBool("Midas Touch", Color.green, MidasTouch.Value, OnGoldMags);
        }

        private static void OnGoldMags(bool obj) {
            MidasTouch.Value = obj;
            MelonPreferences.Save();
        }

        private static void OnSetEnabled(bool value) {
            InfAmmo.Value = value;
            MelonPreferences.Save();
        }

        private static void OnGiveAmmoWhenEmpty(bool value) {
            GiveAmmoWhenEmpty.Value = value;
            MelonPreferences.Save();
        }

        private static void OnInfiniteMags(bool value) {
            InfMags.Value = value;
            MelonPreferences.Save();
        }

        private static void OnAutoChamber(bool value) {
            AutoChamber.Value = value;
            MelonPreferences.Save();
        }
        private static void OnAutoLoad(bool value) {
            AutoLoad.Value = value;
            MelonPreferences.Save();
        }



        [HarmonyPatch(typeof(AmmoInventory), "RemoveCartridge")]
        public class TaxReturn {
            private static void Prefix(AmmoInventory __instance, CartridgeData cartridge, int count) {
                if (InfAmmo.Value) {
                    __instance.AddCartridge(cartridge, count);
                }
            }
        }

        [HarmonyPatch(typeof(InventoryAmmoReceiver), "OnHandGrab")]
        public class AmmoCheckPatch {
            public static void Prefix() {
                if (InfAmmo.Value && GiveAmmoWhenEmpty.Value) {
                    var light = AmmoInventory.Instance.GetCartridgeCount("light");
                    if (light <= 0) {
                        Bankruptcy(AmmoInventory.Instance.lightAmmoGroup);
                    }

                    var medium = AmmoInventory.Instance.GetCartridgeCount("medium");
                    if (medium <= 0) {
                        Bankruptcy(AmmoInventory.Instance.mediumAmmoGroup);
                    }

                    var heavy = AmmoInventory.Instance.GetCartridgeCount("heavy");
                    if (heavy <= 25) {
                        Bankruptcy(AmmoInventory.Instance.heavyAmmoGroup);
                    }
                }
            }


            private static void Bankruptcy(AmmoGroup group) {
                AmmoInventory.Instance.AddCartridge(group, 1);
            }
        }

        [HarmonyPatch(typeof(Magazine), "OnGrab")]
        public class CashBack {
            public static void Postfix() {
                if (InfAmmo.Value) {
                    var leftMag = Player.GetComponentInHand<Magazine>(Player.LeftHand);
                    var rightMag = Player.GetComponentInHand<Magazine>(Player.RightHand);
                    if (leftMag != null)
                        MagMax(leftMag);
                    if (rightMag != null)
                        MagMax(rightMag);
                }
            }

            private static void MagMax(Magazine mag) {
                if (GiveAmmoWhenEmpty.Value) {
                    int magMax = mag.magazineState.magazineData.rounds;
                    int cartridgeCount = AmmoInventory.Instance.GetCartridgeCount(mag.magazineState.cartridgeData);
                    if (cartridgeCount < magMax) {
                        BankStatement(mag, magMax, cartridgeCount);
                    }
                }
                if (InfiniteAmmo.MidasTouch.Value) {
                    MidasTouch(mag);
                }
            }

            private static void MidasTouch(Magazine mag) {
                var renderers = mag.GetComponentsInChildren<Renderer>();
                foreach (Renderer renderer in renderers) {
                    foreach (var material in renderer.materials) {
                        material.mainTexture = null;
                        material.color = Color.yellow;
                        material.SetFloat("_Metallic", 1);
                        material.SetFloat("_Smoothness", 1);
                    }
                }
            }

            private static void BankStatement(Magazine mag, int magMax, int cartridgeCount) {
                if (AmmoInventory.Instance.GetCartridgeCount("light") == cartridgeCount) {
                    Loan(mag, AmmoInventory.Instance.lightAmmoGroup, magMax);
                }
                else if (AmmoInventory.Instance.GetCartridgeCount("medium") == cartridgeCount) {
                    Loan(mag, AmmoInventory.Instance.mediumAmmoGroup, magMax);
                }
                else if (AmmoInventory.Instance.GetCartridgeCount("heavy") == cartridgeCount) {
                    Loan(mag, AmmoInventory.Instance.heavyAmmoGroup, magMax);
                }

            }

            private static void Loan(Magazine mag, AmmoGroup group, int amount) {
                if (AmmoInventory.Instance.GetCartridgeCount(group.KeyName) == 1) {
                    AmmoInventory.Instance.AddCartridge(group, -1);
                }
                AmmoInventory.Instance.AddCartridge(group, amount);
                mag.magazineState.Refill();
            }
        }

        [HarmonyPatch(typeof(Gun), "OnFire")]
        public class CreditCard {
            public static void Postfix(Gun __instance) {
                if (InfAmmo.Value && InfMags.Value && __instance.gameObject.GetComponentInChildren<Magazine>() != null) {
                    __instance.gameObject.GetComponentInChildren<Magazine>().magazineState.Refill();
                }
            }

        }

        [HarmonyPatch(typeof(Gun), "AmmoCount")]
        public class ShotgunCreditCard {
            public static bool Prefix(Gun __instance, ref int __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
            {
                if (InfAmmo.Value && InfMags.Value) {
                    __result = 1;
                    return false;
                }
                else {
                    return true;
                }
            }
        }
        [HarmonyPatch(typeof(Gun), "CheckGunRequirements")]
        public class Paycheck {
            public static void Prefix(Gun __instance) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
            {
                if (InfAmmo.Value && AutoChamber.Value) {
                    __instance.Charge();
                }

                if (InfAmmo.Value && AutoLoad.Value && !__instance._hasMagState) {
                    __instance.InstantLoadAsync();
                }
            }

        }
        [HarmonyPatch(typeof(Gun), "OnTriggerGripAttached")]
        public class DirectDeposit {
            public static void Prefix(Gun __instance) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
            {
                if (InfAmmo.Value && AutoLoad.Value && !__instance._hasMagState) {
                    __instance.InstantLoadAsync();
                }
            }
        }
    }
}
