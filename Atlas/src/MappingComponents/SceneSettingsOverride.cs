using System;
using System.Collections;
using System.Linq;
using Atlas.MappingComponents.Sandbox;
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

            // Reset this flag just in case
            PrefabSpawnPoint.ObjectsCached = false;
            
            // Additively load the mod blank scene to give us the important game bits
            SceneManager.LoadScene("ModBlank_Simple", LoadSceneMode.Additive);
        }

        private IEnumerator Start()
        {
            yield return null;
            
            // Once the scene is loaded we can get the important objects and cache them.
            GameObject[] objects = SceneManager.GetSceneByName("ModBlank_Simple").GetRootGameObjects();
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.ItemSpawner] = objects.First(x => x.name == "ItemSpawner");
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.Destructobin] = objects.First(x => x.name == "Destructobin");
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.SosigSpawner] = objects.First(x => x.name == "SosigSpawner");
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.BangerDetonator] = objects.First(x => x.name == "BangerDetonator");
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.WhizzBangADinger] = objects.First(x => x.name == "WhizzBangADinger2");
            foreach (GameObject obj in PrefabSpawnPoint.CachedObjects.Values) obj.SetActive(false);
            PrefabSpawnPoint.ObjectsCached = true;
        }
    }
}