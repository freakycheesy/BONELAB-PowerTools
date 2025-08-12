using BoneLib;
using BoneLib.BoneMenu;
using MelonLoader;
using PowerTools.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PowerTools
{
    public partial class Main : MelonMod
    {
        public static Page MainPage;
        public static Action OnUpdateEvent; 
        public static Action OnFixedUpdateEvent;
        public static Action OnGUIEvent;
        public static MelonPreferences_Category Preferences { get; private set; }
        public override void OnInitializeMelon() {
            Preferences = MelonPreferences.CreateCategory("PowerTools");
            Preferences.SetFilePath("UserData/freakycheesy.cfg");
            MainPage = Page.Root.CreatePage(ModName, Color.white);

            ToolLoader.LoadTools(defaultMods);
            Hooking.OnLevelLoaded += (_) => { OnSceneAwake(); };
            Hooking.OnLevelUnloaded += Save;
            HarmonyInstance.PatchAll();
        }

        public override void OnUpdate() {
            base.OnUpdate();
            OnUpdateEvent?.Invoke();
        }

        public override void OnFixedUpdate() {
            base.OnFixedUpdate();
            OnFixedUpdateEvent?.Invoke();
        }

        public override void OnGUI() {
            base.OnGUI();
            if(!Application.isMobilePlatform) OnGUIEvent?.Invoke();
        }

        public static List<BaseTool> defaultMods = new List<BaseTool>() {
            new ButtonDisabler(),
            new HealthSettings(),
            new GravityAdjuster(),
            new InfiniteAmmo(),
            new PhysicsTool(),
            new RagdollLegs(),
            new VaultingToggle(),
        };

        private static void OnSceneAwake()
        {
            ToolLoader.ResetTools();
            Save();
        }

        public override void OnApplicationQuit() {
            base.OnApplicationQuit();
            Save();
        }

        public static void Save() {
            MelonPreferences.Save();
        }
    }
}
