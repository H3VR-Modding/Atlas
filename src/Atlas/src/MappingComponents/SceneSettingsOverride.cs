﻿using System;
using System.Collections;
using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents
{
    /// <summary>
    /// This component provides override values for the FVRSceneSettings class
    /// </summary>
    public class SceneSettingsOverride : MonoBehaviour
    { 
        [SerializeField] [HideInInspector]
        private string GameMode = "";

        [Header("Player Affordance")] public bool IsSpawnLockingEnabled = true;
        public bool AreHitBoxesEnabled = false;
        public bool DoesDamageGetRegistered = false;
        public float MaxPointingDistance = 2f;
        public float MaxProjectileRange = 500f;
        public bool ForcesCasingDespawn = false;
        public bool IsGravityForced = false;
        public SimulationOptions.GravityMode ForcedPhysGravity;

        [Header("Locomotion Options")] public bool IsLocoTeleportAllowed = true;
        public bool IsLocoSlideAllowed = true;
        public bool IsLocoDashAllowed = true;
        public bool IsLocoTouchpadAllowed = true;
        public bool IsLocoArmSwingerAllowed = true;
        public bool DoesTeleportUseCooldown = false;
        public bool DoesAllowAirControl = false;
        public bool UseMaxSpeedClamp = false;
        public float MaxSpeedClamp = 3f;

        [Header("Player Catching Options")] public bool UsesPlayerCatcher = true;
        public float CatchHeight = -50f;

        [Header("Player Respawn Options")] public int DefaultPlayerIFF = 0;
        public bool DoesPlayerRespawnOnDeath = true;
        public float PlayerDeathFade = 3;
        public float PlayerRespawnLoadDelay = 3.5f;
        public string SceneToLoadOnDeath = "";
        public bool DoesUseHealthBar = false;
        public bool IsQuickbeltSwappingAllowed = true;
        public bool AreQuickbeltSlotsEnabled = true;
        public bool ConfigQuickbeltOnLoad = false;
        public int QuickbeltToConfig = 0;
        public bool IsSceneLowLight = false;
        public bool IsAmmoInfinite = false;
        public bool AllowsInfiniteAmmoMags = true;
        public bool UsesUnlockSystem = false;

        [Header("Audio Stuff")] public FVRSoundEnvironment DefaultSoundEnvironment = FVRSoundEnvironment.None;
        public float BaseLoudness = 5f;
        public bool UsesWeaponHandlingAISound = false;
        public float MaxImpactSoundEventDistance = 15f;

        private readonly List<AudioSource> _audioSources = new();
        
        private void Awake()
        {
            // Query our gamemode from the AtlasPlugin.
            // This may already be set, in which case we definitely do not want to overwrite it.
            if (string.IsNullOrEmpty(GameMode)) GameMode = AtlasPlugin.CurrentScene!.GameMode;
            
            // Let the scene loader for this game mode take over
            AtlasPlugin.Loaders[GameMode].Awake();
            
            // Apply a fix for any audio sources
            foreach (var source in FindObjectsOfType<AudioSource>())
            {
                if (source.enabled)
                {
                    source.enabled = false;
                    _audioSources.Add(source);
                }
            }
        }

        private IEnumerator Start()
        {
            // Wait one frame for everything to get setup and then let the scene loader take over
            yield return null;
            yield return AtlasPlugin.Loaders[GameMode].Start();
            foreach (var source in _audioSources) source.enabled = true;
        }

        internal void ApplyOverrides(FVRSceneSettings self)
        {
            self.IsSpawnLockingEnabled = IsSpawnLockingEnabled;
            self.AreHitboxesEnabled = AreHitBoxesEnabled;
            self.DoesDamageGetRegistered = DoesDamageGetRegistered;
            self.MaxPointingDistance = MaxPointingDistance;
            self.MaxProjectileRange = MaxProjectileRange;
            self.ForcesCasingDespawn = ForcesCasingDespawn;
            self.IsGravityForced = IsGravityForced;
            self.ForcedPhysGravity = ForcedPhysGravity;
            self.IsLocoTeleportAllowed = IsLocoTeleportAllowed;
            self.IsLocoSlideAllowed = IsLocoSlideAllowed;
            self.IsLocoDashAllowed = IsLocoDashAllowed;
            self.IsLocoTouchpadAllowed = IsLocoTouchpadAllowed;
            self.IsLocoArmSwingerAllowed = IsLocoArmSwingerAllowed;
            self.DoesTeleportUseCooldown = DoesTeleportUseCooldown;
            self.DoesAllowAirControl = DoesAllowAirControl;
            self.UsesMaxSpeedClamp = UseMaxSpeedClamp;
            self.MaxSpeedClamp = MaxSpeedClamp;
            self.UsesPlayerCatcher = UsesPlayerCatcher;
            self.CatchHeight = CatchHeight;
            self.DefaultPlayerIFF = DefaultPlayerIFF;
            self.DoesPlayerRespawnOnDeath = DoesPlayerRespawnOnDeath;
            self.PlayerDeathFade = PlayerDeathFade;
            self.PlayerRespawnLoadDelay = PlayerRespawnLoadDelay;
            self.SceneToLoadOnDeath = SceneToLoadOnDeath;
            self.DoesUseHealthBar = DoesUseHealthBar;
            self.IsQuickbeltSwappingAllowed = IsQuickbeltSwappingAllowed;
            self.AreQuickbeltSlotsEnabled = AreQuickbeltSlotsEnabled;
            self.ConfigQuickbeltOnLoad = ConfigQuickbeltOnLoad;
            self.QuickbeltToConfig = QuickbeltToConfig;
            self.IsSceneLowLight = IsSceneLowLight;
            self.IsAmmoInfinite = IsAmmoInfinite;
            self.AllowsInfiniteAmmoMags = AllowsInfiniteAmmoMags;
            self.UsesUnlockSystem = UsesUnlockSystem;
            self.DefaultSoundEnvironment = DefaultSoundEnvironment;
            self.BaseLoudness = BaseLoudness;
            self.UsesWeaponHandlingAISound = UsesWeaponHandlingAISound;
            self.MaxImpactSoundEventDistance = MaxImpactSoundEventDistance;
        }

        private void OnValidate()
        {
            // Make sure this gets cleared as we now expect it to be empty.
            if (!string.IsNullOrEmpty(GameMode)) GameMode = "";
        }
    }
}