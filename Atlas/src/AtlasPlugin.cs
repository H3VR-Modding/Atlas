using System.Collections;
using System.IO;
using Atlas.Loaders;
using BepInEx;
using Sodalite.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Atlas
{
    [BepInPlugin(Constants.Guid, Constants.Name, Constants.Version)]
    [BepInDependency("nrgill28.Sodalite")]
    [BepInProcess("h3vr.exe")]
    public class AtlasPlugin : BaseUnityPlugin
    {
        private readonly Hooks _hooks = new();
        private AssetBundle? _loadedDebugScene;
        
        public AtlasPlugin()
        {
            // Setup our logger
            Atlas.Logger = Logger;
            
            // Apply our hooks
            _hooks.Hook();
            
            // Wrist menu button for debugging
            WristMenuAPI.Buttons.Add(new WristMenuButton("Debug Load", (_, _) => StartCoroutine(DebugLoadScene())));

            // Register our own loaders
            Atlas.AddLoader(Constants.LoaderSandbox, new SandboxLoader());
            Atlas.AddLoader(Constants.LoaderTakeAndHold, new TakeAndHoldLoader());
        }

        private IEnumerator DebugLoadScene()
        {
            // Load the custom scene
            Atlas.IsSceneLoadInitiatedByMe = true;
            _loadedDebugScene = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, "debugscene"));
            yield return SceneManager.LoadSceneAsync(_loadedDebugScene.GetAllScenePaths()[0]);
        }
    }
}