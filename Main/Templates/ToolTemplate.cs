using BoneLib.BoneMenu;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools {
    public class ToolTemplate : BaseTool {
        public override void Start() {
            base.Start();
        }

        public override void MelonCreator() {
            base.MelonCreator();
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page = Main.Game.CreatePage("Physics Tool", Color.green);
            CreateEnabledBool(Page, this);
        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
        }

        public override void Reset() {
            base.Reset();
        }
    }
}
