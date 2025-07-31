using BoneLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerTools.Tools {
    public class RagdollLegs : BaseTool {

        public override void BoneMenuCreator() {
            Page = Main.Player.CreatePage("Ragdoll Legs", Color.green);
            Page.CreateBool("Mod Toggle", Color.green, _isEnabled, OnSetEnabled);

        }
        public override void OnSetEnabled(bool value) {
            _isEnabled = value;
            if (value) {
                Player.PhysicsRig.PhysicalLegs();
            }
            else {
                Player.PhysicsRig.UnRagdollRig();
            }
        }
        private static bool _isEnabled;
    }
}
