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
            On.FistVR.FVRSceneSettings.Awake += FVRSceneSettingsOnAwake;
            On.FistVR.FVRReverbSystem.Awake += FVRReverbSystemOnAwake;
            On.FistVR.TNH_Manager.Start += TNH_ManagerOnStart;
        }

        public void Unhook()
        {
            On.FistVR.FVRSceneSettings.Awake -= FVRSceneSettingsOnAwake;
            On.FistVR.FVRReverbSystem.Awake -= FVRReverbSystemOnAwake;
            On.FistVR.TNH_Manager.Start -= TNH_ManagerOnStart;
        }

        private void FVRReverbSystemOnAwake(On.FistVR.FVRReverbSystem.orig_Awake orig, FVRReverbSystem self)
        {
            if (Atlas.IsCustomLevel)
            {
                // Copy over reverb stuff
                ReverbSystemOverride reverb = Object.FindObjectOfType<ReverbSystemOverride>();
                if (reverb) reverb.ApplyOverride(self);
            }
            
            // Let orig happen
            orig(self);
        }

        private void FVRSceneSettingsOnAwake(On.FistVR.FVRSceneSettings.orig_Awake orig, FVRSceneSettings self)
        {
            // If we initiated this scene load this is a custom level.
            Atlas.IsCustomLevel = Atlas.IsSceneLoadInitiatedByMe;
            Atlas.IsSceneLoadInitiatedByMe = false;
            
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