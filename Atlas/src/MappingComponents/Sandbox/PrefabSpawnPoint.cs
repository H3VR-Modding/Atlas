using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>
    /// This component will spawn a prefab at it's position
    /// </summary>
    /// <remarks>
    /// Using this component will destroy the game object it's placed on when it spawns the prefab!
    /// </remarks>
    public class PrefabSpawnPoint : MonoBehaviour
    {
        internal static Dictionary<PrefabType, GameObject> CachedObjects = new();
        internal static bool ObjectsCached = false;

        public enum PrefabType
        {
            ItemSpawner,
            Destructobin,
            SosigSpawner,
            WhizzBangADinger,
            BangerDetonator
        }

        public PrefabType Type;

        private IEnumerator Start()
        {
            // Wait until the objects are cached
            while (!ObjectsCached) yield return null;
            
            // Make a copy of the object we want to spawn and set it active
            Instantiate(CachedObjects[Type], transform.position, transform.rotation).SetActive(true);
            
            // Destroy ourselves since we're done
            Destroy(gameObject);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.4f, 0.4f, 0.9f, 0.5f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Vector3 center, size;
            switch (Type)
            {
                case PrefabType.ItemSpawner:
                    center = new Vector3(0f, 0.7f, 0.25f);
                    size = new Vector3(2.3f, 1.2f, 0.5f);
                    break;
                case PrefabType.Destructobin:
                    center = new Vector3(0f, 0.525f, 0f);
                    size = new Vector3(0.65f, 1.05f, 0.65f);
                    break;
                case PrefabType.SosigSpawner:
                    center = new Vector3(0f, 0.11f, 0f);
                    size = new Vector3(0.3f, 0.43f, 0.05f);
                    break;
                case PrefabType.WhizzBangADinger:
                    center = new Vector3(0f, 0.425f, -0.05f);
                    size = new Vector3(0.4f, 0.85f, 0.5f);
                    break;
                case PrefabType.BangerDetonator:
                    center = new Vector3(0f, 0f, 0.1f);
                    size = new Vector3(0.05f, 0.05f, 0.33f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Gizmos.DrawCube(center, size);
            Gizmos.DrawLine(center, center + transform.forward * 0.5f);
        }
    }
}