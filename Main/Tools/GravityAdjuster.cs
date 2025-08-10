using BoneLib.BoneMenu;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools {
    public class GravityAdjuster : BaseTool {
        public static MelonPreferences_Entry<float> MelonPrefGravityValue {
            get; set;
        }

        public override string ToolName => "Gravity Adjuster";

        private static float _gravity = -9.81f;
        //private static float _originalGravity = -9.8f;
        public override void MelonCreator() {
            base.MelonCreator();
            MelonPrefGravityValue = Main.Preferences.CreateEntry("Gravity Adjuster Value", -9.81f);
            if (MelonPrefGravityValue != null) {
                _gravity = MelonPrefGravityValue.Value;
            }
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            //100% a better way to do this but I don't feel like doing it
            FloatElement one = null;
            FloatElement ten = null;

            var pointOne = Page.CreateFloat("Gravity Value (0.1)", Color.green, _gravity, 0.1f, -25f, 25f, (r) => {
                MelonPrefGravityValue.Value = r;
    Main.Save();
                _gravity = r;
                one.Value = r;
                ten.Value = r;
                GravityAdjust();
            });
            one = Page.CreateFloat("Gravity Value (1)", Color.green, _gravity, 1f, -25f, 25f, (r) => {
                MelonPrefGravityValue.Value = r;
    Main.Save();
                pointOne.Value = r;
                ten.Value = r;
                _gravity = r;
                GravityAdjust();
            });
            ten = Page.CreateFloat("Gravity Value (5)", Color.green, _gravity, 5f, -25f, 25f, (r) => {
                MelonPrefGravityValue.Value = r;
    Main.Save();
                pointOne.Value = r;
                one.Value = r;
                _gravity = r;
                GravityAdjust();
            });

        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
            if (value) {
                GravityAdjust();
            }
            else {
                GravityReset();
            }
Main.Save();
        }

        private static void GravityReset() {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }


        public override void Reset() => GravityAdjust();

        public void GravityAdjust() {
            if (ToolEnabled.Value) {
                Physics.gravity = new Vector3(0, _gravity, 0);
            }
        }
    }
}
