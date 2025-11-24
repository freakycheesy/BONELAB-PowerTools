using BoneLib;
using HarmonyLib;
using Il2CppSLZ.Marrow;
using Il2CppSLZ.Marrow.Data;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerTools.Tools {
    public class InfiniteAmmo : BaseTool {
        public static InfiniteAmmo Instance {get; private set;}
        public static MelonPreferences_Entry<bool> InfiniteAmmoEnabled {
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
        public static MelonPreferences_Entry<bool> EjectRefill {
            get; set;
        }
        public static MelonPreferences_Entry<bool> MidasTouch {
            get; set;
        }

        public override string ToolName => "Infinite Ammo";

        public override Color ToolTheme => Color.yellow;

        public override bool HaveEnableToggle => true;

        public override void Start() {
            base.Start();
            Instance = this;
        }

        public override void MelonCreator() {
            base.MelonCreator();
            InfiniteAmmoEnabled = Main.Preferences.CreateEntry("Infinite Ammo", false);
            AutoChamber = Main.Preferences.CreateEntry("AutoChamber", false);
            AutoLoad = Main.Preferences.CreateEntry("AutoLoad", false);
            InfMags = Main.Preferences.CreateEntry("AutoRefill", false);
            EjectRefill = Main.Preferences.CreateEntry("EjectRefill", false);
            MidasTouch = Main.Preferences.CreateEntry("MidasTouch", false);
        }
        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page.CreateBool("Infinte Ammo", ToolTheme, InfiniteAmmoEnabled.Value, OnGiveAmmoWhenEmpty);
            Page.CreateBool("Auto Chamber", ToolTheme, AutoChamber.Value, OnAutoChamber);
            Page.CreateBool("Auto Load Guns", ToolTheme, InfiniteAmmoEnabled.Value, OnAutoLoad);
            Page.CreateBool("Auto Refill", ToolTheme, InfMags.Value, OnInfiniteMags);
            Page.CreateBool("Eject Refill", ToolTheme, EjectRefill.Value, OnEjectRefill);
            Page.CreateBool("Midas Touch", ToolTheme, MidasTouch.Value, OnGoldMags);

        }

        private static void OnGoldMags(bool obj) {
            MidasTouch.Value = obj;
Main.Save();
        }

        private static void OnEjectRefill(bool obj) {
            EjectRefill.Value = obj;
            Main.Save();
        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
Main.Save();
        }

        private static void OnGiveAmmoWhenEmpty(bool value) {
            InfiniteAmmoEnabled.Value = value;
Main.Save();
        }

        private static void OnInfiniteMags(bool value) {
            InfMags.Value = value;
Main.Save();
        }

        private static void OnAutoChamber(bool value) {
            AutoChamber.Value = value;
Main.Save();
        }
        private static void OnAutoLoad(bool value) {
            AutoLoad.Value = value;
Main.Save();
        }

        public static class GunPatches {
            public static bool Enabled {
                get {
                    if (Instance == null)
                        return false;
                    else if (Instance != null && Instance.ToolEnabled == null)
                        return false;
                    else
                        return Instance.ToolEnabled.Value;
                }
            }

            [HarmonyPatch(typeof(AmmoInventory), nameof(AmmoInventory.RemoveCartridge))]
            public static class TaxReturn {
                [HarmonyPrefix]
                public static void Prefix(AmmoInventory __instance, CartridgeData cartridge, int count) {
                    if (Enabled) {
                        __instance.AddCartridge(cartridge, count);
                    }
                }
            }

            [HarmonyPatch(typeof(InventoryAmmoReceiver), nameof(InventoryAmmoReceiver.OnHandGrab))]
            public static class AmmoCheckPatch {
                [HarmonyPrefix]
                public static void Prefix() {
                    if (Enabled && InfiniteAmmoEnabled.Value) {
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

            [HarmonyPatch(typeof(Magazine), nameof(Magazine.OnGrab))]
            public static class CashBack {
                [HarmonyPostfix]
                public static void Postfix() {
                    if (Enabled) {
                        var leftMag = Player.GetComponentInHand<Magazine>(Player.LeftHand);
                        var rightMag = Player.GetComponentInHand<Magazine>(Player.RightHand);
                        if (leftMag != null)
                            MagMax(leftMag);
                        if (rightMag != null)
                            MagMax(rightMag);
                    }
                }

                private static void MagMax(Magazine mag) {
                    if (InfiniteAmmoEnabled.Value) {
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

            [HarmonyPatch(typeof(Gun), nameof(Gun.OnFire))]
            public static class CreditCard {
                [HarmonyPostfix]
                public static void Postfix(Gun __instance) {
                    if (Enabled && InfMags.Value && __instance.gameObject.GetComponentInChildren<Magazine>() != null) {
                        __instance.gameObject.GetComponentInChildren<Magazine>().magazineState.Refill();
                    }
                }

            }

            [HarmonyPatch(typeof(Magazine), nameof(Magazine.OnEject))]
            public static class Withdraw {
                [HarmonyPostfix]
                public static void Postfix(Magazine __instance) {
                    if (Enabled && EjectRefill.Value) {
                        __instance.magazineState.Refill();
                    }
                }

            }


            [HarmonyPatch(typeof(Gun), nameof(Gun.AmmoCount))]
            public static class ShotgunCreditCard {
                [HarmonyPrefix]
                public static bool Prefix(Gun __instance, ref int __result) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
                {
                    if (Enabled && InfMags.Value) {
                        __result = 1;
                        return false;
                    }
                    else {
                        return true;
                    }
                }
            }
            [HarmonyPatch(typeof(Gun), nameof(Gun.CheckGunRequirements))]
            public static class Paycheck {
                [HarmonyPrefix]
                public static void Prefix(Gun __instance) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
                {
                    if (Enabled && AutoChamber.Value) {
                        __instance.Charge();
                    }

                    if (Enabled && AutoLoad.Value && !__instance._hasMagState) {
                        __instance.InstantLoadAsync();
                    }
                }

            }
            [HarmonyPatch(typeof(Gun), nameof(Gun.OnTriggerGripAttached))]
            public static class DirectDeposit {
                [HarmonyPrefix]
                public static void Prefix(Gun __instance) // DO NOT CHANGE __instance OR __result TO ANYTHING ELSE
                {
                    if (Enabled && AutoLoad.Value && !__instance._hasMagState) {
                        __instance.InstantLoadAsync();
                    }
                }
            }
        }
    }
}
