using BoneLib.BoneMenu;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools {
    public class ToolTemplate : BaseTool {
        public override string ToolName => throw new System.NotImplementedException();

        public override Color ToolTheme => throw new System.NotImplementedException();

        public override void Start() {
            base.Start();
        }

        public override void MelonCreator() {
            base.MelonCreator();
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
        }

        public override void Reset() {
            base.Reset();
        }
    }
}
