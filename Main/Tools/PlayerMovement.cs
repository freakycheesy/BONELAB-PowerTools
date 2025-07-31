using BoneLib;
using UnityEngine;

namespace PowerTools.Tools {
    public static class PlayerMovement {

        public static float JumpVelocity;
        public static void Start() {
            Reset();
            BoneMenuCreator();
        }

        public static void BoneMenuCreator() {
            var page = Main.Player.CreatePage("PlayerMovement", Color.green);
            page.CreateFloat("Jump Velocity", Color.green, JumpVelocity, 0.5f, 0, 100, (a) => {
                JumpVelocity = a;
                if(Player.RemapRig) Player.RemapRig.jumpVelocity = JumpVelocity;
            });
        }

        public static void Reset() {
            Hooking.OnSwitchAvatarPostfix -= Hooking_OnSwitchAvatarPostfix;
            Hooking.OnSwitchAvatarPostfix += Hooking_OnSwitchAvatarPostfix;
        }

        private static void Hooking_OnSwitchAvatarPostfix(Il2CppSLZ.VRMK.Avatar obj) {
            if (Player.RemapRig)
                JumpVelocity = Player.RemapRig.jumpVelocity;
        }
    }
}
