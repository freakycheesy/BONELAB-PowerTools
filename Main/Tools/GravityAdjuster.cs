using BoneLib.BoneMenu;
using MelonLoader;
using UnityEngine;
// ReSharper disable AccessToModifiedClosure

namespace PowerTools.Tools
{
    public abstract class GravityAdjuster
    {
        public static MelonPreferences_Entry<float> MelonPrefGravityValue { get; set; }
        private static float _gravity = -9.81f;
        //private static float _originalGravity = -9.8f;
        private static bool _isEnabled;
        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }
        public static void MelonPreferencesCreator()
        {
            MelonPrefGravityValue = Main.MelonPrefCategory.CreateEntry("Gravity Adjuster Value", 9.81f);
            if (MelonPrefGravityValue != null)
            {
                _gravity = MelonPrefGravityValue.Value;
            }
        }
        
        public static void BoneMenuCreator()
        {
            var gravityCustomizer = Main.Category.CreatePage("Gravity Adjuster", Color.green);
        
            gravityCustomizer.CreateBool("Mod Toggle", Color.green, _isEnabled, OnSetEnabled);

            //100% a better way to do this but I don't feel like doing it
            FloatElement one = null;
            FloatElement ten = null;
            
            var pointOne = gravityCustomizer.CreateFloat("Gravity Value (0.1)", Color.green, _gravity, 0.1f, -25f, 25f, (r) =>
            {
                MelonPrefGravityValue.Value = r;
                MelonPreferences.Save();
                _gravity = r;
                one.Value = r;
                ten.Value = r;
                GravityAdjust();
            });
            one = gravityCustomizer.CreateFloat("Gravity Value (1)", Color.green, _gravity, 1f, -25f, 25f, (r) =>
            {
                MelonPrefGravityValue.Value = r;
                MelonPreferences.Save();
                pointOne.Value = r;
                ten.Value = r;
                _gravity = r;
                GravityAdjust();
            });
            ten = gravityCustomizer.CreateFloat("Gravity Value (5)", Color.green, _gravity, 5f, -25f, 25f, (r) =>
            {
                MelonPrefGravityValue.Value = r;
                MelonPreferences.Save();
                pointOne.Value=r;
                one.Value=r;
                _gravity = r;
                GravityAdjust();
            });
            
        }

        private static void OnSetEnabled(bool value)
        {
            _isEnabled = value;
            if (value)
            {
                GravityAdjust();
            }
            else
            {
                GravityReset();
            }
            MelonPreferences.Save();
        }

        private static void GravityReset()
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);
        }


        public static void GravityAdjust()
        {
            if (_isEnabled)
            {
                Physics.gravity = new Vector3(0, _gravity, 0);
            }
        }


    }
}