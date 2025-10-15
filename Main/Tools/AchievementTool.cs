using BoneLib.BoneMenu;
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
        }

        public override void MelonCreator() {
            base.MelonCreator();
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page.CreateEnum("Platform", ToolTheme, Platform, SwitchPlatform);
            Page.CreateFunction("Unlock All Achievements", ToolTheme, UnlockAchievements);
        }

        private void SwitchPlatform(Enum @enum) {
            Platform = (Platform)@enum;
        }

        private void UnlockAchievements() {
            switch (Platform) {
            
                case Platform.Steam:
                    UnlockSteamAchievements();
                    break;
                case Platform.Oculus:
                    UnlockOculusAchievements();
                    break;
            }
        }

        private void UnlockOculusAchievements() {
        }

        private void UnlockSteamAchievements() {
        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
        }

        public override void Reset() {
            base.Reset();
        }
    }
}
