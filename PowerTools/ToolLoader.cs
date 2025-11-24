using MelonLoader;
using PowerTools.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PowerTools {
    public static class ToolLoader {
        public static List<BaseTool> LoadedTools { get; set; } = new List<BaseTool>();
        #region LoadTools
        public static void LoadTool(BaseTool tool) {
            LoadedTools.Add(tool);
            tool.Start();
        }
        public static void LoadTools() {
            LoadedTools.Clear();
            MelonLogger.Msg("Loading Tools from Melons");
            foreach (MelonBase registeredMelon in MelonBase.RegisteredMelons) {
                Utils.LoadAllValid<BaseTool>(registeredMelon.MelonAssembly.Assembly, LoadToolFromType);
            }
            LoadedTools.ToList().ForEach(LoadTool);
        }

        private static void LoadToolFromType(Type type) {
            if (Activator.CreateInstance(type) is not BaseTool tool) {
                return;
            }
            LoadedTools.Add(tool);
        }
        #endregion
        #region Reset
        public static void ResetTools() {
            ResetTools(LoadedTools);
        }
        public static void ResetTools(IEnumerable<BaseTool> tools) {
            foreach (var tool in tools) {
                tool.Reset();
            }
        }
        #endregion
        #region GetTool
        public static BaseTool GetToolInLoadedToolsFromType(Type type) {
            return GetToolInLoadedToolsFromType(type.FullName);
        }
        public static BaseTool GetToolInLoadedToolsFromType(string fullname) {
            foreach (var tool in LoadedTools) {
                if (tool.GetType().FullName == fullname) {
                    MelonLogger.Msg($"Found tool: ({fullname})");
                    return tool;
                }
            }
            MelonLogger.Error($"Could not find Tool with type: ({fullname})");
            return null;
        }
        #endregion
    }
}
