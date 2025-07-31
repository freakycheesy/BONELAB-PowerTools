using BoneLib;
using UnityEngine;

namespace PowerTools.Tools {
    public class PlayerMovement : BaseTool {

        public static float JumpVelocity;
        public new static void Start() {
            Reset();
            BoneMenuCreator();
        }

        public new static void BoneMenuCreator() {
            var page = Main.Player.CreatePage("PlayerMovement", Color.green);
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
            page.CreateFloat("Jump Velocity", Color.green, JumpVelocity, 0.5f, 0, 100, (a) => {
                JumpVelocity = a;
                if(Player.RemapRig) Player.RemapRig.jumpVelocity = JumpVelocity;
            });
        }

        public new static void Reset() {
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
            Hooking.OnSwitchAvatarPostfix -= Hooking_OnSwitchAvatarPostfix;
            Hooking.OnSwitchAvatarPostfix += Hooking_OnSwitchAvatarPostfix;
        }

        private static void Hooking_OnSwitchAvatarPostfix(Il2CppSLZ.VRMK.Avatar obj) {
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
        }
    }
}
