using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Atlas.Loaders;
using BepInEx;
using BepInEx.Logging;
using FistVR;
using Sodalite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Atlas
{
    /// <summary>
    /// Constant values for Atlas
    /// </summary>
    public static class AtlasConstants
    {
        /// <summary>
        /// BepInEx Name of Atlas
        /// </summary>
        public const string Name = "Atlas";
        
        /// <summary>
        /// BepInEx GUID of Atlas
        /// </summary>
        public const string Guid = "nrgill28.Atlas";
        
        /// <summary>
        /// Version of Atlas
        /// </summary>
        public const string Version = "0.1.0";

        internal const string LoaderSandbox = "sandbox";
        internal const string LoaderTakeAndHold = "takeandhold";
    }
    
    [BepInPlugin(AtlasConstants.Guid, AtlasConstants.Name, AtlasConstants.Version)]
    [BepInDependency(SodaliteConstants.Guid, SodaliteConstants.Version)]
    [BepInProcess("h3vr.exe")]
    public partial class AtlasPlugin : BaseUnityPlugin
    {
        /// <summary>
        /// True if the currently loaded level is custom, false if it's vanilla
        /// </summary>
        public static bool IsPlayingCustomLevel { get; internal set; }
        
        /// <summary>
        /// The SceneInfo for the currently loaded custom scene or null if the current scene is not custom
        /// </summary>
        public static CustomSceneInfo? CurrentScene { get; internal set; }
        
        internal static readonly Dictionary<string, ISceneLoader> Loaders = new();
        internal static readonly List<CustomSceneInfo> SceneInfos = new();
        internal new static ManualLogSource Logger = null!;
        internal static IDisposable? LeaderboardLock;
        
        public void Awake()
        {
            // Setup our logger
            Logger = base.Logger;
            
            // Apply our hooks
            ApplyHooks();

            // Register our own loaders
            RegisterLoader(AtlasConstants.LoaderSandbox, new SandboxLoader());
            RegisterLoader(AtlasConstants.LoaderTakeAndHold, new TakeAndHoldLoader());

            // Callback for when a scene is loaded
            SceneManager.sceneLoaded += (scene, _) => StartCoroutine(SceneManagerOnSceneLoaded(scene));
        }

        /// <summary>
        /// Registers a new scene loader
        /// </summary>
        /// <param name="mode">The ID of the mode</param>
        /// <param name="loader">The loader to register</param>
        public static void RegisterLoader(string mode, ISceneLoader loader)
        {
            Loaders.Add(mode, loader);
        }

        /// <summary>
        /// Registers a scene with Atlas given a file path
        /// </summary>
        /// <param name="sceneBundleFilePath">Path of the scene's asset bundle</param>
        /// <exception cref="FileNotFoundException">File does not exist</exception>
        public static void RegisterScene(string sceneBundleFilePath)
        {
            // Expect a file (or throw)
            FileInfo file = new(sceneBundleFilePath);
            Logger.LogDebug($"Attempting to load scene from bundle at: {sceneBundleFilePath}");
            if (!file.Exists) throw new FileNotFoundException("Path points to non-existing file!");

            // Add a new scene info to our list
            CustomSceneInfo info = new(file);
            SceneInfos.Add(info);
            Logger.LogInfo($"Registered {info.DisplayName} by {info.Author}.");
        }
        
        private IEnumerator SceneManagerOnSceneLoaded(Scene scene)
        {
            // We only use this callback for setting up the main menu level selector panel
            if (scene.name != "MainMenu3") yield break;

            // TODO: This is WurstMod code, might want to clean this up and use a different sandbox level selection technique
            GameObject sceneScreenBase = GameObject.Find("SceneScreen_GDC");
            GameObject labelBase = GameObject.Find("Label_Description_1_Title (5)");

            // Get a circle.
            List<Vector3> screenPositions = new();
            float CircleX(float x) => (14.13f * Mathf.Cos(Mathf.Deg2Rad * x)) - 0.39f;
            float CircleZ(float z) => (14.13f * Mathf.Sin(Mathf.Deg2Rad * z)) - 2.98f;
            for (int ii = 0; ii < 360; ii += 13)
            for (int jj = 0; jj < 4; jj++)
                screenPositions.Add(new Vector3(CircleX(ii), 0.5f + (jj * 2), CircleZ(ii)));

            // Trimming positions we don't want and order by -z.
            screenPositions = screenPositions.Where(x => x.z < -7f).OrderByDescending(x => -x.z)
                .ThenBy(x => Mathf.Abs(x.y - 4.15f)).ToList();

            // For compatibility with WurstMod only take one side of these. WurstMod will take the other.
            screenPositions = screenPositions.Where(p => p.x > 0).ToList();

            // Modded Levels label.
            GameObject moddedScenesLabel = Instantiate(labelBase, labelBase.transform.parent);
            moddedScenesLabel.transform.position = new Vector3(4f, 8.3f, -15.5f);
            moddedScenesLabel.transform.localEulerAngles = new Vector3(-180f, -30f, 180f);
            moddedScenesLabel.GetComponent<Text>().text = "Atlas Sandbox Scenes";

            // Create the screens for each scene
            CustomSceneInfo[] sandboxScenes = SceneInfos.Where(x => x.Mode == AtlasConstants.LoaderSandbox).ToArray();
            int mainTex = Shader.PropertyToID("_MainTex");
            for (int i = 0; i < sandboxScenes.Length; i++)
            {
                // Create and position properly. Rename so patch can handle it properly.
                MainMenuScenePointable screen = Instantiate(sceneScreenBase, sceneScreenBase.transform.parent)
                    .GetComponent<MainMenuScenePointable>();
                screen.transform.position = screenPositions[i];
                screen.transform.localEulerAngles = new Vector3(0,
                    180 - (Mathf.Rad2Deg * Mathf.Atan(-screen.transform.position.x / screen.transform.position.z)), 0);
                screen.transform.localScale = 0.5f * screen.transform.localScale;

                // Make sure scaling is set correctly.
                MainMenuScenePointable screenComponent = screen.GetComponent<MainMenuScenePointable>();
                screenComponent.m_startScale = screen.transform.localScale;

                // Setup the screen with our values
                CustomSceneInfo sceneInfo = sandboxScenes[i];
                CustomMainMenuSceneDef def = ScriptableObject.CreateInstance<CustomMainMenuSceneDef>();
                def.Name = sceneInfo.DisplayName;
                def.Type = sceneInfo.Author;
                def.Desciption = sceneInfo.Description;
                def.Image = sceneInfo.ThumbnailSprite;
                def.CustomSceneInfo = sceneInfo;

                screen.Def = def;
                screen.GetComponent<MeshRenderer>().material.SetTexture(mainTex, sceneInfo.ThumbnailTexture);
            }
        }
    }
}