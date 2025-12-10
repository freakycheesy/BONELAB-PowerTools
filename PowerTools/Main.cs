using BoneLib;
using BoneLib.BoneMenu;
using MelonLoader;
using UnityEngine;

namespace PowerTools
{
    public partial class Main : MelonMod
    {
        public static bool CanUseSteamworks {
            get; internal set;
        } = false;
        public static Page MainPage;
        public static Action OnUpdateEvent; 
        public static Action OnFixedUpdateEvent;
        public static Action OnGUIEvent;
        public static MelonPreferences_Category Preferences { get; private set; }
        public override void OnInitializeMelon() {
            CanUseSteamworks = !Application.isMobilePlatform;
            if (!CanUseSteamworks) {
                MelonLogger.Error("BONELAB FUSION NEEDED FOR STEAMWORKS SINCE IM LAZY AF TO PORT IT ");
            }
            if (!GameSafe()) {
                return;
            }
            Preferences = MelonPreferences.CreateCategory("PowerTools", "Power Tools");
            MainPage = Page.Root.CreatePage(ModName, Color.white);
            Hooking.OnLevelLoaded += (_) => { OnSceneAwake(); };
            Hooking.OnLevelUnloaded += Save;
            ToolLoader.LoadTools();
        }

        private bool GameSafe() {
            Dictionary<KeyValuePair<string, string>, string> BADMODS = new();
            BADMODS.Add(new("Fusion Protector", "James Reborn"), "Fusion Protector is a Fusion Backdoor made by a controversial/non trustworthy figure in the bonelab community who caused tons of drama by making a cheat client");
            foreach (var mod in BADMODS) {
                if (FindMelon(mod.Key.Key, mod.Key.Value) != null) {
                    MelonLogger.Error($"BAD MOD FOUND,\n UNINSTALL IT FAST IN YOUR MODS FOLDER ON PC\nMELONLOADER-STRESSLEVELZERO-BONELAB-MODS ON QUEST!!!\n[Mod:{mod.Key.ToString()}] [Reason:{mod.Value}]");
                    Environment.Exit(0);
                    Application.Quit();
                    return false;
                }
            }
            return true;
        }

        public override void OnUpdate() {
            base.OnUpdate();
            OnUpdateEvent?.Invoke();
        }

        public override void OnFixedUpdate() {
            base.OnFixedUpdate();
            OnFixedUpdateEvent?.Invoke();
        }

        public override void OnGUI() {
            base.OnGUI();
            if(CanUseSteamworks) OnGUIEvent?.Invoke();
        }

        private static void OnSceneAwake()
        {
            ToolLoader.ResetTools();
            Save();
        }

        public override void OnApplicationQuit() {
            Save();
            base.OnApplicationQuit();
        }

        public static void Save() {
            MelonPreferences.Save();
        }
    }
}
