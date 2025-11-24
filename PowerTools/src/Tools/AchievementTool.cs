using BoneLib.BoneMenu;
using BoneLib.Notifications;
using Il2CppOculus.Platform;
using LabFusion.Extensions;
using MelonLoader;
using Steamworks;
using UnityEngine;

namespace PowerTools.Tools {
    public class AchievementTool : BaseTool {
        public override string ToolName => "Achievement Tool";
        public override Color ToolTheme => Color.red + Color.yellow;

        public override bool HaveEnableToggle => false;

        public static Page AchievementsPage;
        public static Action InitiatedSteam;

        public override void Start() {
            base.Start();
            if (!Main.CanUseSteamworks)
                return;
            InitiatedSteam += AddAchievements;
        }

        private void AddAchievements() {
            foreach (var achievement in SteamUserStats.Achievements) {
                var page = AchievementsPage.CreatePage($"{achievement.Name}\n({achievement.Identifier})", Color.green);
                page.CreateFunction("Unlock", Color.green, ()=>UnlockAchievement(achievement));
                page.CreateFunction("Lock", Color.green, ()=>LockAchievement(achievement));
            }
        }

        private void LockAchievement(Steamworks.Data.Achievement achievement) {
            achievement.Clear();
            SteamUserStats.StoreStats();
            SteamUserStats.RequestCurrentStats();
        }

        private void UnlockAchievement(Steamworks.Data.Achievement achievement) {
            achievement.Trigger(true);
            SteamUserStats.StoreStats();
            SteamUserStats.RequestCurrentStats();
        }

        public override void MelonCreator() {
            base.MelonCreator();
            if (!Main.CanUseSteamworks)
                return;
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            if (!Main.CanUseSteamworks) {
                Page.CreateFunction("Steam Only", ToolTheme, () => {
                    Notification womp = new Notification();
                    womp.ShowTitleOnPopup = true;
                    womp.PopupLength = 0.1f;
                    womp.Title = "guess what";
                    womp.Message = "impossible on oculus";
                });
                return;
            }
            Page.CreateFunction("Initiate Steam", ToolTheme, InitPlatform);
            Page.CreateFunction("Unlock All Achievements", ToolTheme, UnlockAchievements);
            Page.CreateFunction("Lock All Achievements", ToolTheme, LockAchievements);
            AchievementsPage = Page.CreatePage("Achievements", Color.green, 12);
        }

        private void InitPlatform() {
            SteamClient.Init(1592190, true);
            InitiatedSteam?.Invoke();
        }

        private void LockAchievements() {
            UnlockSteamAchievements();
        }

        private void UnlockAchievements() {
            LockSteamAchievements();
        }

        private void UnlockSteamAchievements() {
            SteamUserStats.Achievements.ForEach(UnlockAchievement);
        }

        private void LockSteamAchievements() {
            SteamUserStats.Achievements.ForEach(LockAchievement);
        }


        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
        }

        public override void Reset() {
            base.Reset();
        }
    }
}
