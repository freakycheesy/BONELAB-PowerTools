using BoneLib;
using MelonLoader;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PowerTools.Tools {
    public static class PlayerMovement {

        public static float JumpVelocity;
        public static void Start() {
            BoneMenuCreator();
        }

        public static void BoneMenuCreator() {
            JumpVelocity = Player.RemapRig.jumpVelocity;
            var page = Main.Player.CreatePage("PlayerMovement", Color.green);

            page.CreateFloat("Jump Velocity", Color.green, JumpVelocity, 0.5f, 0, 100, (a) => {
                JumpVelocity = a;
                Player.RemapRig.jumpVelocity = JumpVelocity;
            });
        }
    }
}
