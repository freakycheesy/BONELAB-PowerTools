using Il2CppSLZ.Interaction;
using MelonLoader;
using UnityEngine;

namespace PowerTools.Tools {
    public class ButtonDisabler : BaseTool{
        public override void Start() {
            base.Start();
        }

        public override void MelonCreator() {
            base.MelonCreator();
            _endOfLevelButton = Main.Preferences.CreateEntry("Disable end of level button", false);
        }

        public override void BoneMenuCreator() {
            base.BoneMenuCreator();
            Page = Main.Game.CreatePage("Button Disabler", Color.green);
            CreateEnabledBool(Page, this);
            Page.CreateBool("Disable Next Level Button", Color.green, _endOfLevelButton.Value, OnEndOfLevelButtonEnabled);
        }

        public override void OnSetEnabled(bool value) {
            base.OnSetEnabled(value);
            DisableButtons();
        }

        public override void Reset() {
            base.Reset();
            DisableButtons();
        }
        public static MelonPreferences_Entry<bool> _endOfLevelButton {
            get; set;
        }

        public void DisableButtons() {
            var objectsWithKeyword = UnityEngine.Object.FindObjectsOfType<Transform>(true);
            foreach (Transform obj in objectsWithKeyword) {
                if (obj.name.Contains("FLOORS") || obj.name.Contains("LoadButtons") || obj.name.Contains("prop_bigButton") || obj.name.Contains("INTERACTION")) {

                    for (int i = 0; i < obj.childCount; i++) {
                        Transform child = obj.GetChild(i);
                        var buttonToggle = child.GetComponent<ButtonToggle>();
                        if (buttonToggle != null && ToolEnabled.Value) {
                            if (_endOfLevelButton.Value) {
                                buttonToggle.enabled = false;
                            }
                            else if (!_endOfLevelButton.Value) {
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
            _endOfLevelButton.Value = value;
            MelonPreferences.Save();
            DisableButtons();
        }
    }
}
