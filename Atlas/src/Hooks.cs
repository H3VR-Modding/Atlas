using Atlas.MappingComponents;
using Atlas.MappingComponents.TakeAndHold;
using FistVR;
using UnityEngine;

namespace Atlas
{
    internal class Hooks
    {
        public void Hook()
        {
            On.FistVR.FVRSceneSettings.Start += FVRSceneSettingsOnStart;
            On.FistVR.TNH_Manager.Start += TNH_ManagerOnStart;
        }

        public void Unhook()
        {
            On.FistVR.FVRSceneSettings.Start -= FVRSceneSettingsOnStart;
            On.FistVR.TNH_Manager.Start -= TNH_ManagerOnStart;
        }

        private void FVRSceneSettingsOnStart(On.FistVR.FVRSceneSettings.orig_Start orig, FVRSceneSettings self)
        {
            // If we're loading a custom level
            if (Atlas.IsCustomLevel)
            {
                // Copy over all of the scene settings into this before initializing
                SceneSettingsOverride settings = Object.FindObjectOfType<SceneSettingsOverride>();
                if (settings) settings.ApplyOverrides(self);
                
                // Then we can re-initialize 
                GM.Instance.InitScene();
            }
            
            // Let the original start do it's thing afterwards
            orig(self);
        }
        
        private void TNH_ManagerOnStart(On.FistVR.TNH_Manager.orig_Start orig, TNH_Manager self)
        {
            // If we're loading a custom level
            if (Atlas.IsCustomLevel)
            {
                TNH_ManagerOverride overrides = Object.FindObjectOfType<TNH_ManagerOverride>();
                if (overrides) overrides.ApplyOverrides(self);
                else Atlas.Logger.LogError("TNH_Manager overrides were missing, you need one in your scene!");
            }

            orig(self);
        }
    }
}