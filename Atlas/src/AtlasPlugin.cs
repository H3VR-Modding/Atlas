using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Atlas.Loaders;
using BepInEx;
using FistVR;
using Sodalite;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Atlas
{
    [BepInPlugin(AtlasConstants.Guid, AtlasConstants.Name, AtlasConstants.Version)]
    [BepInDependency(SodaliteConstants.Guid, SodaliteConstants.Version)]
    [BepInProcess("h3vr.exe")]
    public partial class AtlasPlugin : BaseUnityPlugin
    {
        private readonly List<CustomSceneInfo> _sceneInfos = new();

        public AtlasPlugin()
        {
            // Setup our logger
            Atlas.Logger = Logger;

            // Apply our hooks
            On.FistVR.FVRSceneSettings.Awake += FVRSceneSettingsOnAwake;
            On.FistVR.TNH_Manager.Start += TNH_ManagerOnStart;
            On.FistVR.MainMenuScreen.LoadScene += MainMenuScreenOnLoadScene;

            // Register our own loaders
            Atlas.AddLoader(AtlasConstants.LoaderSandbox, new SandboxLoader());
            Atlas.AddLoader(AtlasConstants.LoaderTakeAndHold, new TakeAndHoldLoader());

            // Callback for when a scene is loaded
            SceneManager.sceneLoaded += (scene, _) => StartCoroutine(SceneManagerOnSceneLoaded(scene));

            // DEBUG: Load the debug scene
            RegisterScene(new FileInfo(Path.Combine(Paths.PluginPath, "samplesandbox")));
        }

        private void RegisterScene(FileSystemInfo handle)
        {
            // Expect a file (or throw)
            FileInfo file = handle as FileInfo ?? throw new ArgumentException("Expected path to be a file!");
            if (!file.Exists) throw new FileNotFoundException("Path points to non-existing file!");

            // Add a new scene info to our list
            _sceneInfos.Add(new CustomSceneInfo(file));
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
            screenPositions = screenPositions.Where(x => x.z < -7f).OrderByDescending(x => -x.z).ThenBy(x => Mathf.Abs(x.y - 4.15f)).ToList();

            // Modded Levels label.
            GameObject moddedScenesLabel = Instantiate(labelBase, labelBase.transform.parent);
            moddedScenesLabel.transform.position = new Vector3(0f, 8.3f, -17.1f);
            moddedScenesLabel.transform.localEulerAngles = new Vector3(-180f, 0f, 180f);
            moddedScenesLabel.GetComponent<Text>().text = "Atlas Sandbox Scenes";

            // Create the screens for each scene
            CustomSceneInfo[] sandboxScenes = _sceneInfos.Where(x => x.Mode == AtlasConstants.LoaderSandbox).ToArray();
            int mainTex = Shader.PropertyToID("_MainTex");
            for (int i = 0; i < sandboxScenes.Length; i++)
            {
                // Create and position properly. Rename so patch can handle it properly.
                MainMenuScenePointable screen = Instantiate(sceneScreenBase, sceneScreenBase.transform.parent).GetComponent<MainMenuScenePointable>();
                screen.transform.position = screenPositions[i];
                //screen.transform.LookAt(Vector3.zero, Vector3.up);
                screen.transform.localEulerAngles = new Vector3(0, 180 - (Mathf.Rad2Deg * Mathf.Atan(-screen.transform.position.x / screen.transform.position.z)), 0);
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