using Il2CppSLZ.Interaction;
using MelonLoader;
using UnityEngine;
using Object = UnityEngine.Object;

namespace PowerTools.Tools {
    public class ButtonDisabler : BaseTool {
        public override void Start() {
            base.Start();
        }

        public override void MelonCreator() {
            base.MelonCreator();
            EndOfLevelButton = Main.Preferences.CreateEntry("Disableendoflevelbutton", false);
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page.CreateBool("Disable Next Level Button", ToolTheme, EndOfLevelButton.Value, OnEndOfLevelButtonEnabled);
        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
            DisableButtons();
        }

        public override void Reset() {
            base.Reset();
            DisableButtons();
        }
        public static MelonPreferences_Entry<bool> EndOfLevelButton {
            get; set;
        }
        public override string ToolName {
            get => "Button Disabler";
        }

        public override Color ToolTheme => Color.red + (Color.yellow / 2);

        public void DisableButtons() {
            var objectsWithKeyword = Object.FindObjectsOfType<Transform>(true);
            foreach (Transform obj in objectsWithKeyword) {
                if (obj.name.Contains("FLOORS") || obj.name.Contains("LoadButtons") || obj.name.Contains("prop_bigButton") || obj.name.Contains("INTERACTION")) {

                    for (int i = 0; i < obj.childCount; i++) {
                        Transform child = obj.GetChild(i);
                        var buttonToggle = child.GetComponent<ButtonToggle>();
                        if (buttonToggle != null && ToolEnabled.Value) {
                            if (EndOfLevelButton.Value) {
                                buttonToggle.enabled = false;
                            }
                            else if (!EndOfLevelButton.Value) {
                                if (!child.name.Contains("prop_bigButton_NEXTLEVEL")) {
                                    buttonToggle.enabled = false;
                                }
                                if (child.name.Contains("prop_bigButton_NEXTLEVEL")) {
                                    buttonToggle.enabled = true;
                                }
                            }
                        }
                        else if (buttonToggle != null && !ToolEnabled.Value) {
                            buttonToggle.enabled = true;
                        }
                    }
                }
            }
        }

        private void OnEndOfLevelButtonEnabled(bool value) {
            EndOfLevelButton.Value = value;
            Main.Save();
            DisableButtons();
        }
    }
}
