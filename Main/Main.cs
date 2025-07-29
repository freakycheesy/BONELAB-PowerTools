using BoneLib;
using BoneLib.BoneMenu;
using MelonLoader;
using MelonLoader.Utils;
using PowerTools.Tools;
using System.IO;
using UnityEngine;

namespace PowerTools
{
    internal partial class Main : MelonMod
    {
        private static Page Category;
        public static Page Player;
        public static Page Game;


        public static MelonPreferences_Category MelonPrefCategory { get; private set; }

        public static readonly string PowerToolsPath = Path.Combine(MelonEnvironment.UserDataDirectory, "PowerTools");

        public override void OnInitializeMelon() {
            Hooking.OnLevelLoaded += (_) => { OnSceneAwake(); };
            MelonPrefCategory = MelonPreferences.CreateCategory("Power Tools");

            Category = Page.Root.CreatePage(
                "<color=#00FF72>P</color>" +
                "<color=#00FF80>o</color>" +
                "<color=#00FF8D>w</color>" +
                "<color=#00FF99>e</color>" +
                "<color=#00FFA5>r</color>" +
                "<color=#00FFB0> </color>" +
                "<color=#00FFBA>T</color>" +
                "<color=#00FFC3>o</color>" +
                "<color=#00FFCC>o</color>" +
                "<color=#00FFD4>l</color>" +
                "<color=#00FFD4>s</color>", Color.white);
            Player = Category.CreatePage("Player", Color.green);
            Game = Category.CreatePage("Game", Color.green);

            StartTools();
        }

        private static void StartTools() {
            DeathSettings.Start();

            ButtonDisabler.Start();

            RagdollOnDeath.Start();

            VaultingToggle.Start();

            GravityAdjuster.Start();

            //Loadouts.Start();

            InfiniteAmmo.Start();
            PlayerMovement.Start();

            RagdollLegs.Start();
        }

        private static void OnSceneAwake()
        {
            DeathSettings.DeathTimeSetter();
            
            ButtonDisabler.DisableButtons();
            
            RagdollOnDeath.OnSetEnabled(RagdollOnDeath.RagdollOnDeathIsEnabled.Value);
            
            GravityAdjuster.GravityAdjust();
        }

        public override void OnApplicationQuit() {
            base.OnApplicationQuit();
            MelonPreferences.Save();
        }

        public override void OnUpdate() // Only run tools under OnUpdate() if theres no alternatives and if an alternative is found please PR it
        {
            //BugoSpray.BugoRemover(); this is stupid I only made it for a bit
            
            //BrickShooter.dfgijkhokjnhgrshjikhjiusgihuseg(); IT DOESNT EVEN SHOOT BRICKS??????????? WHY
        }


    }
}
