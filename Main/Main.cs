using BoneLib;
using BoneLib.BoneMenu;
using MelonLoader;
using PowerTools.Tools;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PowerTools
{
    public partial class Main : MelonMod
    {
        public static Page MainPage;
        public static Page Player;
        public static Page Game;

        public static MelonPreferences_Category Preferences { get; private set; }
        public override void OnInitializeMelon() {
            Hooking.OnLevelLoaded += (_) => { OnSceneAwake(); };
            Preferences = MelonPreferences.CreateCategory("Power Tools");

            MainPage = Page.Root.CreatePage(Name, Color.white);

            Player = MainPage.CreatePage("Player", Color.green);
            Game = MainPage.CreatePage("Game", Color.green);

            ToolLoader.LoadTools(defaultMods);
        }

        public static List<BaseTool> defaultMods = new List<BaseTool>() {
            new ButtonDisabler(),
            new DeathSettings(),
            new GravityAdjuster(),
            new InfiniteAmmo(),
            new PhysicsTool(),
            new PlayerMovement(),
            new RagdollLegs(),
            new VaultingToggle(),
        };

        private static void OnSceneAwake()
        {
            ToolLoader.ResetTools();
            if(ToolLoader.loadedTools.Count < 1)
                ToolLoader.LoadTools(defaultMods);
        }

        public override void OnApplicationQuit() {
            base.OnApplicationQuit();
            MelonPreferences.Save();
        }
    }
}
