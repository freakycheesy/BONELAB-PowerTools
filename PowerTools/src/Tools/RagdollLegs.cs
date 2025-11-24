using BoneLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerTools.Tools {
    public class RagdollLegs : BaseTool {

        public override void OnSetEnabled(bool value) {
            if (value) {
                Player.PhysicsRig.PhysicalLegs();
            }
            else {
                Player.PhysicsRig.UnRagdollRig();
            }
        }

        public override string ToolName => "Ragdoll Legs";

        public override Color ToolTheme => Color.cyan;

        public override bool HaveEnableToggle => true;
    }
}
