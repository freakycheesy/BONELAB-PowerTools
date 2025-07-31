using BoneLib;
using UnityEngine;

namespace PowerTools.Tools {
    public class PlayerMovement : BaseTool {

        public static float JumpVelocity = 500;

        public override void Start() {
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
            base.Start();
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page = Main.Player.CreatePage("PlayerMovement", Color.green);
            CreateEnabledBool(Page, this);
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
            Page.CreateFloat("Jump Velocity", Color.green, JumpVelocity, 500f, 0, int.MaxValue, OnJumpVelocity);
        }

        private void OnJumpVelocity(float a) {
            JumpVelocity = a;
            if (Player.RemapRig && ToolEnabled.Value)
                Player.RemapRig.jumpVelocity = JumpVelocity;
        }

        public override void Reset() {      
            base.Reset();
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
            OnJumpVelocity(JumpVelocity);
            Hooking.OnSwitchAvatarPostfix -= Hooking_OnSwitchAvatarPostfix;
            Hooking.OnSwitchAvatarPostfix += Hooking_OnSwitchAvatarPostfix;
        }

        private static void Hooking_OnSwitchAvatarPostfix(Il2CppSLZ.VRMK.Avatar obj) {
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
        }
    }
}
