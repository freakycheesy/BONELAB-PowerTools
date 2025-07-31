using BoneLib.BoneMenu;
using MelonLoader;
using System.Reflection;
using UnityEngine;

namespace PowerTools.Tools {
    public class BaseTool {
        public Page Page;
        public MelonPreferences_Entry<bool> ToolEnabled;
        public virtual void Start() {
            try {
                MelonCreator();
                BoneMenuCreator();
            }
            catch (System.Exception e) {
                MelonLogger.Error($"Error when loading tool: ({GetType().FullName})");
                MelonLogger.Error(e);
            }
            MelonLogger.Error($"Loaded tool: ({GetType().FullName})");
        }

        public virtual void MelonCreator() {
            if (ToolEnabled != null)
                return;
            ToolEnabled = Main.Preferences.CreateEntry($"{GetType().FullName}.Enabled", false);
        }

        public virtual void BoneMenuCreator() {
            if (Page != null)
                return;
        }

        public virtual void OnSetEnabled(bool value) {
            ToolEnabled.Value = value;
        }

        public virtual void Reset() {
            if (ToolEnabled == null)
                return;
        }

        public static void CreateEnabledBool(Page page, BaseTool baseTool) {
            page.CreateBool("Enabled", Color.green, baseTool.ToolEnabled.Value, (a) => baseTool.ToolEnabled.Value = a);
        }
    }
}
