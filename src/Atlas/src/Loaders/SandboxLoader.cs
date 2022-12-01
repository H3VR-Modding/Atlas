using System.Collections;
using System.Linq;
using Atlas.MappingComponents.Sandbox;
using FistVR;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Atlas.Loaders
{
    public class SandboxLoader : ISceneLoader
    {
        public virtual void Awake()
        {
            // Reset this flag just in case
            PrefabSpawnPoint.ObjectsCached = false;
            
            // Additively load the mod blank scene to give us the important game bits
            SceneManager.LoadScene("ModBlank_Simple", LoadSceneMode.Additive);
        }

        public virtual IEnumerator Start()
        {
            // Once the scene is loaded we can get the important objects and cache them.
            GameObject[] objects = SceneManager.GetSceneByName("ModBlank_Simple").GetRootGameObjects();
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.ItemSpawner] = IM.Prefab_ItemSpawner;
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.Destructobin] = objects.First(x => x.name == "Destructobin");
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.SosigSpawner] = objects.First(x => x.name == "SosigSpawner");
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.BangerDetonator] = objects.First(x => x.name == "BangerDetonator");
            PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.WhizzBangADinger] = objects.First(x => x.name == "WhizzBangADinger2");
            foreach (GameObject obj in PrefabSpawnPoint.CachedObjects.Values) obj.SetActive(false);
            PrefabSpawnPoint.ObjectsCached = true;

            yield break;
        }
    }
}
