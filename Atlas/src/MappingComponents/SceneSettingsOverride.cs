using FistVR;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Atlas.MappingComponents
{
    /// <summary>
    /// This component provides override values for the FVRSceneSettings class
    /// </summary>
    public class SceneSettingsOverride : MonoBehaviour
    {
        [Header("Player Affordance")] public bool IsSpawnLockingEnabled = true;
        public bool AreHitBoxesEnabled = false;
        public bool DoesDamageGetRegistered = false;
        public float MaxPointingDistance = 1f;
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
        
        private void Awake()
        {
            // If there's a camera left somewhere in the scene remove it. 
            Camera camera = FindObjectOfType<Camera>();
            if (camera) Destroy(camera.gameObject);

            // Additively load the mod blank scene to give us the important game bits
            SceneManager.LoadScene("ModBlank_Simple", LoadSceneMode.Additive);
        }
    }
}