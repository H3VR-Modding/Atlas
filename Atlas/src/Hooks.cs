using Atlas.MappingComponents;
using FistVR;
using UnityEngine;

namespace Atlas
{
    internal class Hooks
    {
        public void Hook()
        {
            On.FistVR.FVRSceneSettings.Start += FVRSceneSettingsOnStart;
        }

        public void Unhook()
        {
            On.FistVR.FVRSceneSettings.Start -= FVRSceneSettingsOnStart;
        }

        private void FVRSceneSettingsOnStart(On.FistVR.FVRSceneSettings.orig_Start orig, FVRSceneSettings self)
        {
            // If we're loading a custom level
            if (Atlas.IsCustomLevel)
            {
                // Copy over all of the scene settings into this before initializing
                SceneSettingsOverride settings = Object.FindObjectOfType<SceneSettingsOverride>();
                if (settings)
                {
                    self.IsSpawnLockingEnabled = settings.IsSpawnLockingEnabled;
                    self.AreHitboxesEnabled = settings.AreHitBoxesEnabled;
                    self.DoesDamageGetRegistered = settings.DoesDamageGetRegistered;
                    self.MaxPointingDistance = settings.MaxPointingDistance;
                    self.MaxProjectileRange = settings.MaxProjectileRange;
                    self.ForcesCasingDespawn = settings.ForcesCasingDespawn;
                    self.IsGravityForced = settings.IsGravityForced;
                    self.ForcedPhysGravity = settings.ForcedPhysGravity;
                    self.IsLocoTeleportAllowed = settings.IsLocoTeleportAllowed;
                    self.IsLocoSlideAllowed = settings.IsLocoSlideAllowed;
                    self.IsLocoDashAllowed = settings.IsLocoDashAllowed;
                    self.IsLocoTouchpadAllowed = settings.IsLocoTouchpadAllowed;
                    self.IsLocoArmSwingerAllowed = settings.IsLocoArmSwingerAllowed;
                    self.DoesTeleportUseCooldown = settings.DoesTeleportUseCooldown;
                    self.DoesAllowAirControl = settings.DoesAllowAirControl;
                    self.UsesMaxSpeedClamp = settings.UseMaxSpeedClamp;
                    self.MaxSpeedClamp = settings.MaxSpeedClamp;
                    self.UsesPlayerCatcher = settings.UsesPlayerCatcher;
                    self.CatchHeight = settings.CatchHeight;
                    self.DefaultPlayerIFF = settings.DefaultPlayerIFF;
                    self.DoesPlayerRespawnOnDeath = settings.DoesPlayerRespawnOnDeath;
                    self.PlayerDeathFade = settings.PlayerDeathFade;
                    self.PlayerRespawnLoadDelay = settings.PlayerRespawnLoadDelay;
                    self.SceneToLoadOnDeath = settings.SceneToLoadOnDeath;
                    self.DoesUseHealthBar = settings.DoesUseHealthBar;
                    self.IsQuickbeltSwappingAllowed = settings.IsQuickbeltSwappingAllowed;
                    self.AreQuickbeltSlotsEnabled = settings.AreQuickbeltSlotsEnabled;
                    self.ConfigQuickbeltOnLoad = settings.ConfigQuickbeltOnLoad;
                    self.QuickbeltToConfig = settings.QuickbeltToConfig;
                    self.IsSceneLowLight = settings.IsSceneLowLight;
                    self.IsAmmoInfinite = settings.IsAmmoInfinite;
                    self.AllowsInfiniteAmmoMags = settings.AllowsInfiniteAmmoMags;
                    self.UsesUnlockSystem = settings.UsesUnlockSystem;
                    self.DefaultSoundEnvironment = settings.DefaultSoundEnvironment;
                    self.BaseLoudness = settings.BaseLoudness;
                    self.UsesWeaponHandlingAISound = settings.UsesWeaponHandlingAISound;
                    self.MaxImpactSoundEventDistance = settings.MaxImpactSoundEventDistance;
                }
                
                // Then we can re-initialize 
                GM.Instance.InitScene();
            }
            
            // Let the original start do it's thing afterwards
            orig(self);
        }
    }
}