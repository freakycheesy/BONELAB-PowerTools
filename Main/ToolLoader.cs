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
        public static List<BaseTool> loadedTools = new List<BaseTool>();
        #region LoadTools
        public static void LoadTools(List<BaseTool> tools) {
            LoadTools(tools.ToArray());
        }
        public static void LoadTools(BaseTool[] tools) {
            loadedTools.AddRange(tools);
            foreach (var tool in loadedTools) {
                tool.Start();
            }
        }
        #endregion
        #region Reset
        public static void ResetTools() {
            ResetTools(loadedTools);
        }
        public static void ResetTools(List<BaseTool> tools) {
            ResetTools(tools.ToArray());
        }
        public static void ResetTools(BaseTool[] tools) {
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
            foreach (var tool in loadedTools) {
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
