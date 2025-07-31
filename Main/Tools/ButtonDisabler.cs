using Il2CppSLZ.Interaction;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    internal static class ButtonDisabler
    {
        public static MelonPreferences_Entry<bool> ButtonDisablerIsEnabled { get;  set; }
        public static MelonPreferences_Entry<bool> _endOfLevelButton { get; set; }
        public static void Reset() => DisableButtons();
        public static void Start() {
            MelonPreferencesCreator();
            BoneMenuCreator();
        }
        public static void MelonPreferencesCreator()
        {
            ButtonDisablerIsEnabled = Main.MelonPrefCategory.CreateEntry("ButtonDisablerIsEnabled", false);
            _endOfLevelButton = Main.MelonPrefCategory.CreateEntry("Disable end of level button", false);
        }

        public static void BoneMenuCreator()
        {
            var deathTimeCustomizer = Main.Game.CreatePage("Button Disabler ", Color.green);

            deathTimeCustomizer.CreateBool("Mod Toggle", Color.green, ButtonDisablerIsEnabled.Value, OnSetEnabled);

            deathTimeCustomizer.CreateBool("Disable Next Level Button", Color.green, _endOfLevelButton.Value, OnEndOfLevelButtonEnabled);
        }
        public static void DisableButtons()
        {
            var objectsWithKeyword = Object.FindObjectsOfType<Transform>(true);
            foreach (Transform obj in objectsWithKeyword)
            {
                if (obj.name.Contains("FLOORS") || obj.name.Contains("LoadButtons") || obj.name.Contains("prop_bigButton") || obj.name.Contains("INTERACTION"))
                {

                    for (int i = 0; i < obj.childCount; i++)
                    {
                        Transform child = obj.GetChild(i);
                        var buttonToggle = child.GetComponent<ButtonToggle>();
                        if (buttonToggle != null && ButtonDisablerIsEnabled.Value)
                        {
                            if (_endOfLevelButton.Value)
                            {
                                buttonToggle.enabled = false;
                            }
                            else if (!_endOfLevelButton.Value)
                            {
                                if (!child.name.Contains("prop_bigButton_NEXTLEVEL"))
                                {
                                    buttonToggle.enabled = false;
                                }
                                if (child.name.Contains("prop_bigButton_NEXTLEVEL"))
                                {
                                    buttonToggle.enabled = true;
                                }
                            }
                        }
                        else if (buttonToggle != null && !ButtonDisablerIsEnabled.Value)
                        {
                            buttonToggle.enabled = true;
                        }
                    }
                }
            }
        }

        private static void OnSetEnabled(bool value)
        {
            ButtonDisablerIsEnabled.Value = value;



            DisableButtons();
        }

        private static void OnEndOfLevelButtonEnabled(bool value)
        {
            _endOfLevelButton.Value = value;
            MelonPreferences.Save();
            DisableButtons();
        }
    }
}
