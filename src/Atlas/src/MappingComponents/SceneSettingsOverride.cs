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

        /// <summary>When true, allows the player to spawnlock items in their quickbelt.</summary>
        [Header("Player Affordance")]
        public bool IsSpawnLockingEnabled = true;

        /// <summary>When true, enables the player's damage hitboxes.</summary>
        /// <remarks>Has similar functionality to <see cref="DoesDamageGetRegistered"/></remarks>
        public bool AreHitBoxesEnabled;

        /// <summary>When true, the player can receive damage.</summary>
        /// <remarks>Has similar functionality to <see cref="AreHitBoxesEnabled"/></remarks>
        public bool DoesDamageGetRegistered;

        /// <summary>The maximum distance which the player can point at pointable objects.</summary>
        public float MaxPointingDistance = 2f;

        /// <summary>The maximum range that projectiles can travel in this scene. Once they pass this distance they are deleted.</summary>
        public float MaxProjectileRange = 500f;

        /// <summary>When true, spent casings will always despawn even if the player has specified otherwise in the settings.</summary>
        public bool ForcesCasingDespawn;

        /// <summary>When true, this scene will force the use of a specific gravity setting, ignoring what the player has specified in the settings.</summary>
        /// <seealso cref="ForcedPhysGravity"/>
        public bool IsGravityForced;

        /// <summary>The gravity setting to use when <see cref="IsGravityForced"/> is enabled.</summary>
        /// <seealso cref="IsGravityForced"/>
        public SimulationOptions.GravityMode ForcedPhysGravity;

        /// <summary>When true, allows the use of teleport locomotion.</summary>
        /// <seealso cref="IsLocoSlideAllowed"/>
        /// <seealso cref="IsLocoDashAllowed"/>
        /// <seealso cref="IsLocoTouchpadAllowed"/>
        /// <seealso cref="IsLocoArmSwingerAllowed"/>
        [Header("Locomotion Options")]
        public bool IsLocoTeleportAllowed = true;

        /// <summary>When true, allows the use of slide locomotion.</summary>
        /// <seealso cref="IsLocoTeleportAllowed"/>
        /// <seealso cref="IsLocoDashAllowed"/>
        /// <seealso cref="IsLocoTouchpadAllowed"/>
        /// <seealso cref="IsLocoArmSwingerAllowed"/>
        public bool IsLocoSlideAllowed = true;

        /// <summary>When true, allows the use of dash locomotion.</summary>
        /// <seealso cref="IsLocoTeleportAllowed"/>
        /// <seealso cref="IsLocoSlideAllowed"/>
        /// <seealso cref="IsLocoTouchpadAllowed"/>
        /// <seealso cref="IsLocoArmSwingerAllowed"/>
        public bool IsLocoDashAllowed = true;


        /// <summary>When true, allows the use of touchpad (twinstick) locomotion.</summary>
        /// <seealso cref="IsLocoTeleportAllowed"/>
        /// <seealso cref="IsLocoSlideAllowed"/>
        /// <seealso cref="IsLocoDashAllowed"/>
        /// <seealso cref="IsLocoArmSwingerAllowed"/>
        public bool IsLocoTouchpadAllowed = true;

        /// <summary>When true, allows the use of arm swinger locomotion.</summary>
        /// <seealso cref="IsLocoTeleportAllowed"/>
        /// <seealso cref="IsLocoSlideAllowed"/>
        /// <seealso cref="IsLocoDashAllowed"/>
        /// <seealso cref="IsLocoTouchpadAllowed"/>
        public bool IsLocoArmSwingerAllowed = true;

        /// <summary>When true, teleport locomotion options will require a short delay between teleports.</summary>
        public bool DoesTeleportUseCooldown;

        /// <summary>When true, allows the player to control themselves better while in the air.</summary>
        public bool DoesAllowAirControl;

        /// <summary>When true, clamps the player's maximum speed to <see cref="MaxSpeedClamp"/>.</summary>
        public bool UseMaxSpeedClamp;

        /// <summary>The maximum speed the player can move when <see cref="UseMaxSpeedClamp"/> is true.</summary>
        public float MaxSpeedClamp = 3f;

        /// <summary>When true, the player will be moved to the scene's reset point if they fall below <see cref="CatchHeight"/>.</summary>
        [Header("Player Catching Options")]
        public bool UsesPlayerCatcher = true;

        /// <summary>Falling below this height with <see cref="UsesPlayerCatcher"/> enabled will move the player back to the reset point.</summary>
        public float CatchHeight = -50f;

        /// <summary>The initial IFF of the player.</summary>
        [Header("Player Respawn Options")]
        public int DefaultPlayerIFF;

        /// <summary>When false, instead of respawning the player on death, the scene specified by <see cref="SceneToLoadOnDeath"/> will be loaded.</summary>
        public bool DoesPlayerRespawnOnDeath = true;

        /// <summary>The duration of the visual fade effect when the player dies.</summary>
        public float PlayerDeathFade = 3;

        /// <summary>The delay between when the player dies and when they are respawned.</summary>
        /// <remarks>
        /// This runs concurrently with <see cref="PlayerDeathFade"/>, with the default values of 3 and 3.5 the player
        /// will have their vision faded to black for 3 seconds and then half a second later will respawn.
        /// </remarks>
        public float PlayerRespawnLoadDelay = 3.5f;

        /// <summary>The scene to load when the player dies if <see cref="DoesPlayerRespawnOnDeath"/> is false.</summary>
        public string SceneToLoadOnDeath = "";

        /// <summary>When true, the health bar will be displayed to the player.</summary>
        public bool DoesUseHealthBar;

        /// <summary>When true, the player will be allowed to change their quickbelt while in the scene.</summary>
        public bool IsQuickbeltSwappingAllowed = true;

        /// <summary>When true, the player will be given quickbelt slots.</summary>
        /// <remarks>This is used in the main menu, where the player does not have the quickbelt.</remarks>
        public bool AreQuickbeltSlotsEnabled = true;

        /// <summary>When true, a specific quickbelt designated by <see cref="QuickbeltToConfig"/> will be switched to on scene load.</summary>
        public bool ConfigQuickbeltOnLoad;

        /// <summary>The index of the quickbelt to load on scene start when <see cref="ConfigQuickbeltOnLoad"/> is true.</summary>
        public int QuickbeltToConfig;

        /// <summary>When true, light effects like flashlights and muzzle flash have increased intensity.</summary>
        public bool IsSceneLowLight;

        /// <summary>When true, the player will have infinite ammo. Magazines will never empty allowing the player to fire forever.</summary>
        public bool IsAmmoInfinite;

        /// <summary>When true, enables magazines which are set to have specific ammo to work.</summary>
        public bool AllowsInfiniteAmmoMags = true;

        /// <summary>When true, the item spawner will operate as it does in M.E.A.T.S., where all firearms are locked by default and must be purchased.</summary>
        public bool UsesUnlockSystem;

        /// <summary>The base 'loudness' of the scene. Audio events with a loudness less than this are ignored by AI entities.</summary>
        [Header("Audio Stuff")]
        public float BaseLoudness = 5f;
        
        /// <summary>When true, the player handling weapons will emit audio events detectable by AI entities.</summary>
        public bool UsesWeaponHandlingAISound;
        
        /// <summary>The maximum distance which object collision sounds will alert AI entities.</summary>
        public float MaxImpactSoundEventDistance = 15f;

        private readonly List<AudioSource> _audioSources = new();

        private void Awake()
        {
            // Update the current scene
            AtlasPlugin.CurrentScene = AtlasPlugin.LastLoadedScene;

            // Query our game mode from the AtlasPlugin.
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
            self.DefaultSoundEnvironment = FVRSoundEnvironment.None;
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