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
            Preferences = MelonPreferences.CreateCategory("PowerTools", "Power Tools");
            MainPage = Page.Root.CreatePage(ModName, Color.white);
            Hooking.OnLevelLoaded += (_) => { OnSceneAwake(); };
            Hooking.OnLevelUnloaded += Save;
            ToolLoader.LoadTools();
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
