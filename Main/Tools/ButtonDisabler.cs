using Il2CppSLZ.Interaction;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools
{
    internal static class ButtonDisabler
    {
        private static bool _endOfLevelButton;

        private static MelonPreferences_Entry<bool> MelonPrefEnabled { get;  set; }
        private static bool ButtonDisablerIsEnabled { get; set; }
        private static MelonPreferences_Entry<bool> MelonPrefEndOfLevelButton { get; set; }

        public static void MelonPreferencesCreator()
        {
            MelonPrefEnabled = Main.MelonPrefCategory.CreateEntry("ButtonDisablerIsEnabled", false);
            ButtonDisablerIsEnabled = MelonPrefEnabled.Value;
            MelonPrefEndOfLevelButton = Main.MelonPrefCategory.CreateEntry("Disable end of level button", false);

            if (MelonPrefEndOfLevelButton != null)
            {
                _endOfLevelButton = MelonPrefEndOfLevelButton.Value;
            }
        }

        public static void BoneMenuCreator()
        {
            var deathTimeCustomizer = Main.Category.CreatePage("Button Disabler ", Color.green);

            deathTimeCustomizer.CreateBool("Mod Toggle", Color.green, ButtonDisablerIsEnabled, OnSetEnabled);

            deathTimeCustomizer.CreateBool("Disable Next Level Button", Color.green, _endOfLevelButton, OnEndOfLevelButtonEnabled);
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
                        if (buttonToggle != null && ButtonDisablerIsEnabled)
                        {
                            if (_endOfLevelButton)
                            {
                                buttonToggle.enabled = false;
                            }
                            else if (!_endOfLevelButton)
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
                        else if (buttonToggle != null && !ButtonDisablerIsEnabled)
                        {
                            buttonToggle.enabled = true;
                        }
                    }
                }
            }
        }

        private static void OnSetEnabled(bool value)
        {
            ButtonDisablerIsEnabled = value;
            MelonPrefEnabled.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
            DisableButtons();
        }

        private static void OnEndOfLevelButtonEnabled(bool value)
        {
            _endOfLevelButton = value;
            MelonPrefEndOfLevelButton.Value = value;
            Main.MelonPrefCategory.SaveToFile(false);
            DisableButtons();
        }
    }
}
