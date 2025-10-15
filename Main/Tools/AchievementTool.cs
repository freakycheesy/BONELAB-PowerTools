using BoneLib.BoneMenu;
using MelonLoader;
using System;
using UnityEngine;

namespace PowerTools.Tools {
    public class AchievementTool : BaseTool {
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
            Page.CreateFunction("Unlock Achievements", ToolTheme, UnlockAchievements);
        }

        private void UnlockAchievements() {
            throw new NotImplementedException();
        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
        }

        public override void Reset() {
            base.Reset();
        }
    }
}
