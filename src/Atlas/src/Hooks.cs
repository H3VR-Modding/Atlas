using System;
using System.Collections;
using System.Linq;
using Atlas.MappingComponents;
using Atlas.MappingComponents.TakeAndHold;
using FistVR;
using Sodalite.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
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
            On.FistVR.TNH_UIManager.Start += TNH_UIManagerOnStart;
            On.FistVR.SceneLoader.LoadMG += SceneLoaderOnLoadMG;
            On.FistVR.TNH_UIManager.UpdateLevelSelectDisplayAndLoader +=
                TNH_UIManagerOnUpdateLevelSelectDisplayAndLoader;
            
            // These three patches prevent the soft-locking of a map if there aren't enough Sosig spawn points
            On.FistVR.TNH_SupplyPoint.SpawnTakeEnemyGroup += TNH_SupplyPointOnSpawnTakeEnemyGroup;
            On.FistVR.TNH_HoldPoint.SpawnTakeEnemyGroup += TNH_HoldPoint_SpawnTakeEnemyGroup;
            On.FistVR.TNH_HoldPoint.SpawnHoldEnemyGroup += TNH_HoldPoint_SpawnHoldEnemyGroup;
            
            
            // Patch this so that it doesn't throw an error if it's one frame too early.
            On.FistVR.FVRReverbSystem.CheckPlayerEnvironment += (orig, self) =>
            {
                if (GM.CurrentPlayerBody) orig(self);
            };
            
            // Same here
            On.FistVR.FVRAmbienceController.Update += (orig, self) =>
            {
                if (GM.CurrentPlayerBody) orig(self);
            };
#endif
        }

#if RUNTIME
        private static void FVRSceneSettingsOnAwake(On.FistVR.FVRSceneSettings.orig_Awake orig, FVRSceneSettings self)
        {
            // If there's a scene settings override component somewhere in the scene we'll say this is a custom scene.
            SceneSettingsOverride settings = FindObjectOfType<SceneSettingsOverride>();
            if (settings)
            {


                // Copy over all of the scene settings into this before initializing
                settings.ApplyOverrides(self);

                // Then we can re-initialize 
                GM.Instance.InitScene();

                // if we don't currently have a leaderboard lock get a new one
                LeaderboardLock ??= LeaderboardAPI.LeaderboardDisabled.TakeLock();
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
                    AssetBundleCreateRequest
                        request = AssetBundle.LoadFromFileAsync(sceneInfo.SceneBundleFile.FullName);

                    while (request.progress < 0.9f)
                    {
                        // Change the button text to loading since it may take a sec to load the scene bundle
                        self.LoadSceneButton.GetComponentInChildren<Text>().text =
                            $"Loading {request.progress * 1.11f:P1}";
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

        private void TNH_UIManagerOnStart(On.FistVR.TNH_UIManager.orig_Start orig, TNH_UIManager self)
        {
            foreach (var sceneInfo in RegisteredScenes.Where(s => s.DisplayMode == AtlasConstants.LoaderTakeAndHold))
                self.Levels.Add(new CustomLevelData(sceneInfo));

            orig(self);
        }

        private void TNH_UIManagerOnUpdateLevelSelectDisplayAndLoader(
            On.FistVR.TNH_UIManager.orig_UpdateLevelSelectDisplayAndLoader orig, TNH_UIManager self)
        {
            if (self.GetLevelData(self.CurLevelID) is CustomLevelData customLevelData)
            {
                LastLoadedScene = customLevelData.SceneInfo;
                LeaderboardLock ??= LeaderboardAPI.LeaderboardDisabled.TakeLock();
            }
            else
            {
                LastLoadedScene = null;
                LeaderboardLock?.Dispose();
                LeaderboardLock = null;
            }

            orig(self);
        }

        private void SceneLoaderOnLoadMG(On.FistVR.SceneLoader.orig_LoadMG orig, SceneLoader self)
        {
            // This hotdog is also used in MeatGrinder so check to make sure we're not in there first
            if (SceneManager.GetActiveScene().name != "TakeAndHold_Lobby_2" || LastLoadedScene is null)
            {
                orig(self);
                return;
            }

            IEnumerator LoadBundleThenScene(CustomSceneInfo sceneInfo)
            {
                if (!sceneInfo.SceneBundle)
                {
                    // Create the load request
                    AssetBundleCreateRequest
                        request = AssetBundle.LoadFromFileAsync(sceneInfo.SceneBundleFile.FullName);
                    yield return request;
                    sceneInfo.SceneBundle = request.assetBundle;
                }

                self.LevelName = sceneInfo.SceneBundle!.GetAllScenePaths()[0];
                orig(self);
            }

            StartCoroutine(LoadBundleThenScene(LastLoadedScene));
        }
        
        private void TNH_SupplyPointOnSpawnTakeEnemyGroup(On.FistVR.TNH_SupplyPoint.orig_SpawnTakeEnemyGroup orig, TNH_SupplyPoint self)
        {
            try
            {
                orig(self);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ignore the exception and log a warning.
                int idx = self.M.SupplyPoints.IndexOf(self);
                Logger.LogWarning("Supply point " + idx + " does not have enough Sosig defense spawn points! Needed " + self.T.NumGuards + ", has " + self.SpawnPoints_Sosigs_Defense.Count);
            }
        }
        
        private void TNH_HoldPoint_SpawnTakeEnemyGroup(On.FistVR.TNH_HoldPoint.orig_SpawnTakeEnemyGroup orig, TNH_HoldPoint self)
        {
            try
            {
                orig(self);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ignore the exception and log a warning.
                int idx = self.M.HoldPoints.IndexOf(self);
                Logger.LogWarning("Hold point " + idx + " does not have enough Sosig defense spawn points! Needed " + self.T.NumGuards + ", has " + self.SpawnPoints_Sosigs_Defense.Count);
            }
        }
        
        private void TNH_HoldPoint_SpawnHoldEnemyGroup(On.FistVR.TNH_HoldPoint.orig_SpawnHoldEnemyGroup orig, TNH_HoldPoint self)
        {
            try
            {
                orig(self);
            }
            catch (ArgumentOutOfRangeException)
            {
                // Ignore the exception and log a warning.
                int idx = self.M.HoldPoints.IndexOf(self);
                Logger.LogWarning("Hold point " + idx + " does not have enough Sosig attack spawn points!");
                
                // This variable also has to be set
                self.m_isFirstWave = false;
            }
        }
#endif
    }
}
