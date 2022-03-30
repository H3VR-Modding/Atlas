using System.Collections;
using Atlas.MappingComponents.Sandbox;
using FistVR;
using UnityEngine.SceneManagement;

namespace Atlas.Loaders
{
    public class TakeAndHoldLoader : ISceneLoader
    {
        public void Awake()
        {
            // Reset this flag just in case
            PrefabSpawnPoint.ObjectsCached = false;
            
            // Additively load the mod blank scene to give us the important game bits
            SceneManager.LoadScene("ModBlank_Take&Hold", LoadSceneMode.Additive);
        }

        public IEnumerator Start()
        {
            // If we have the item spawner enabled then we can allow that prefab in the prefab spawn point
            if (GM.TNH_Manager.ItemSpawnerMode == TNH_ItemSpawnerMode.On)
                PrefabSpawnPoint.CachedObjects[PrefabSpawnPoint.PrefabType.ItemSpawner] = GM.TNH_Manager.ItemSpawner;
            PrefabSpawnPoint.ObjectsCached = true;

            yield break;
        }
    }
}