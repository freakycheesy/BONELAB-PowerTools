using BoneLib.BoneMenu;
using Steamworks;
using System;
using UnityEngine;

namespace PowerTools.Tools {
    public enum Platform : byte {
        Steam,
        Oculus,
    }
    public class AchievementTool : BaseTool {
        public static Platform Platform = Platform.Steam;
        public override string ToolName => "Achievement Tool";

        public override Color ToolTheme => Color.red + Color.yellow;

        public override void Start() {
            base.Start();
            Platform = !Application.isMobilePlatform ? Platform.Steam : Platform.Oculus;
        }

        public override void MelonCreator() {
            base.MelonCreator();
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page.CreateEnum("Platform", ToolTheme, Platform, SwitchPlatform);
            Page.CreateFunction("Initiate Platform", ToolTheme, InitPlatform);
            Page.CreateFunction("Unlock All Achievements", ToolTheme, UnlockAchievements);
            Page.CreateFunction("Lock All Achievements", ToolTheme, LockAchievements);
        }

        private void InitPlatform() {
            switch (Platform) {
                case Platform.Steam:
                    SteamClient.Init(1592190, true);
                    break;
                case Platform.Oculus:
                    break;
            }
        }

        private void LockAchievements() {
            switch (Platform) {

                case Platform.Steam:
                    UnlockSteamAchievements();
                    break;
                case Platform.Oculus:
                    UnlockOculusAchievements();
                    break;
            }
        }

        private void SwitchPlatform(Enum @enum) {
            Platform = (Platform)@enum;
        }

        private void UnlockAchievements() {
            switch (Platform) {
                case Platform.Steam:
                    LockSteamAchievements();
                    break;
                case Platform.Oculus:
                    LockOculusAchievements();
                    break;
            }
        }

        private void UnlockOculusAchievements() {
        }

        private void UnlockSteamAchievements() {
            foreach (var achievement in SteamUserStats.Achievements) {
                achievement.Trigger();
            }
            SteamUserStats.StoreStats();
        }

        private void LockOculusAchievements() {
        }

        private void LockSteamAchievements() {
            foreach (var achievement in SteamUserStats.Achievements) {
                achievement.Clear();
            }
            SteamUserStats.StoreStats();
        }


        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
        }

        public override void Reset() {
            base.Reset();
        }
    }
}
