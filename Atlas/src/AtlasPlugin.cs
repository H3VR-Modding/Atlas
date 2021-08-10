using System.Collections;
using System.IO;
using BepInEx;
using FistVR;
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
        private Hooks _hooks = new();
        
        public AtlasPlugin()
        {
            // Apply our hooks
            _hooks.Hook();
            
            // Wrist menu button for debugging
            WristMenuAPI.Buttons.Add(new WristMenuButton("Debug Load", () => StartCoroutine(DebugLoadScene())));
        }

        private IEnumerator DebugLoadScene()
        {
            // Load the custom scene
            Atlas.IsCustomLevel = true;
            AssetBundle sceneBundle = AssetBundle.LoadFromFile(Path.Combine(Paths.PluginPath, "debugscene"));
            yield return SceneManager.LoadSceneAsync(sceneBundle.GetAllScenePaths()[0]);
        }
    }
}