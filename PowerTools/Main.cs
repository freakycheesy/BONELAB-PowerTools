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
        public static bool CanUseSteamworks => FindMelon("LabFusion", "Lakatrazz") != null && !Application.isMobilePlatform;
        public static Page MainPage;
        public static Action OnUpdateEvent; 
        public static Action OnFixedUpdateEvent;
        public static Action OnGUIEvent;
        public static MelonPreferences_Category Preferences { get; private set; }
        public override void OnInitializeMelon() {
            if (!CanUseSteamworks) {
                MelonLogger.Error("BONELAB FUSION NEEDED FOR STEAMWORKS SINCE IM LAZY AF TO PORT IT ");
            }
            Preferences = MelonPreferences.CreateCategory("PowerTools", "Power Tools");
            Preferences.SetFilePath("UserData/freakycheesy.cfg");
            MainPage = Page.Root.CreatePage(ModName, Color.white);
            ToolLoader.LoadTools(defaultMods);
            Hooking.OnLevelLoaded += (_) => { OnSceneAwake(); };
            Hooking.OnLevelUnloaded += Save;
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

        public static BaseTool[] defaultMods = new BaseTool[] { new ButtonDisabler(),
            new AchievementTool(),
            new HealthSettings(),
            new GravityAdjuster(),
            new InfiniteAmmo(),
            new PhysicsTool(),
            new RagdollLegs(),
            new VaultingToggle()
        };

        private static void OnSceneAwake()
        {
            ToolLoader.ResetTools();
            Save();
        }

        public override void OnApplicationQuit() {
            Save();
            base.OnApplicationQuit();
        }

        public static void Save() {
            Preferences.SaveToFile();
            MelonPreferences.Save();
        }
    }
}
