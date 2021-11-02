﻿using System.Collections;
using Atlas.MappingComponents;
using Atlas.MappingComponents.TakeAndHold;
using FistVR;
using Sodalite.Api;
using UnityEngine;
using UnityEngine.UI;

namespace Atlas
{
    public partial class AtlasPlugin
    {
        private void ApplyHooks()
        {
#if RUNTIME
            On.FistVR.FVRSceneSettings.Awake += FVRSceneSettingsOnAwake;
            On.FistVR.TNH_Manager.Start += TNH_ManagerOnStart;
            On.FistVR.MainMenuScreen.LoadScene += MainMenuScreenOnLoadScene;
#endif
        }
        
#if RUNTIME
        private static void FVRSceneSettingsOnAwake(On.FistVR.FVRSceneSettings.orig_Awake orig, FVRSceneSettings self)
        {
            // If there's a scene settings override component somewhere in the scene we'll say this is a custom scene.
            SceneSettingsOverride settings = FindObjectOfType<SceneSettingsOverride>();
            if (settings)
            {
                // Update the current scene
                CurrentScene = LastLoadedScene;
                
                // Copy over all of the scene settings into this before initializing
                settings.ApplyOverrides(self);

                // Then we can re-initialize 
                GM.Instance.InitScene();

                // if we don't currently have a leaderboard lock get a new one
                LeaderboardLock ??= LeaderboardAPI.GetLeaderboardDisableLock();
            }
            else
            {
                // Update the current scene
                CurrentScene = null;
                
                // If we have a leaderboard lock dispose of it since this is now a vanilla scene
                LeaderboardLock?.Dispose();
                LeaderboardLock = null;
            }

            // Let the original start do it's thing afterwards
            orig(self);
        }

        private static void TNH_ManagerOnStart(On.FistVR.TNH_Manager.orig_Start orig, TNH_Manager self)
        {
            // If we're loading a custom level
            if (CurrentScene != null)
            {
                TNH_ManagerOverride overrides = FindObjectOfType<TNH_ManagerOverride>();
                if (overrides) overrides.ApplyOverrides(self);
                else Logger.LogError("TNH_Manager overrides were missing, you need one in your scene!");
            }

            orig(self);
        }

        private void MainMenuScreenOnLoadScene(On.FistVR.MainMenuScreen.orig_LoadScene orig, MainMenuScreen self)
        {
            IEnumerator LoadBundleThenScene(CustomSceneInfo sceneInfo)
            {
                // We need to load the bundle first before we can do anything
                if (!sceneInfo.SceneBundle)
                {
                    // Create the load request
                    AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(sceneInfo.SceneBundleFile.FullName);

                    while (request.progress < 0.9f)
                    {
                        // Change the button text to loading since it may take a sec to load the scene bundle
                        self.LoadSceneButton.GetComponentInChildren<Text>().text = $"Loading {request.progress*1.11f:P1}";
                        yield return null;
                    }
                    
                    sceneInfo.SceneBundle = request.assetBundle;
                }

                // Update the scene name on the def so it points to the right (now loaded) scene
                self.m_def.SceneName = sceneInfo.SceneBundle!.GetAllScenePaths()[0];

                // Finally let original method take over.
                LastLoadedScene = sceneInfo;
                orig(self);
            }

            // If this is a custom scene we have to load the asset bundle first
            if (self.m_def is CustomMainMenuSceneDef sceneDef)
            {
                StartCoroutine(LoadBundleThenScene(sceneDef.CustomSceneInfo));
            }
            else orig(self);
        }
#endif
    }
}