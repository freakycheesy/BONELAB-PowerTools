using BoneLib.BoneMenu;
using MelonLoader;
using System.Reflection;
using UnityEngine;

namespace PowerTools.Tools {
    public abstract class BaseTool {
        public Page Page;
        public MelonPreferences_Entry<bool> ToolEnabled;
        public bool started;
        public abstract string ToolName {
            get;
        }
        public abstract Color ToolTheme {
            get;
        }
        public virtual void Start() {
            if (started)
                return;
            MelonCreator();
            BoneMenuCreator();
            MelonLogger.Msg($"Loaded tool: ({ToolName})");
            started = true;
        }

        public virtual void MelonCreator() {
            ToolEnabled = Main.Preferences.CreateEntry($"{ToolName} Enabled", false);
        }

        public virtual void BoneMenuCreator() {
            Page = Main.MainPage.CreatePage(ToolName, ToolTheme);
            Page.CreateBool("Enabled", ToolTheme, ToolEnabled.Value, (a) => ToolEnabled.Value = a);
        }

        public virtual void OnSetEnabled(bool value) {
            ToolEnabled.Value = value;
        }

        public virtual void Reset() {
            if (ToolEnabled == null)
                return;
        }
    }
}
