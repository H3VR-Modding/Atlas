using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>
    /// This component will spawn a prefab at it's position
    /// </summary>
    public class PrefabSpawnPoint : MonoBehaviour
    {
        public enum PrefabType
        {
            ItemSpawner,
            Destructobin,
            SosigSpawner
        }

        public PrefabType Type;
    }
}