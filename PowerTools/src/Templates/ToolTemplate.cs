using UnityEngine;

namespace PowerTools.Tools {
    public abstract class ToolTemplate : BaseTool {
        public override string ToolName => "";

        public override Color ToolTheme => Color.green;

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
