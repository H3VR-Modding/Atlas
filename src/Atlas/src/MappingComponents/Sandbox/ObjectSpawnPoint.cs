using System.Collections;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>
    /// Spawn point component for an Anvil object
    /// </summary>
    public class ObjectSpawnPoint : MonoBehaviour
    {
        /// <summary>
        /// The object id of the prefab to spawn. 
        /// </summary>
        public string ObjectId = "";
        
        /// <summary>
        /// When true, the object will be spawned immediately on scene load. Otherwise it can be spawned by calling
        /// <see cref="Spawn"/> from another script or via a UnityEvent.
        /// </summary>
        public bool SpawnOnStart = true;

        private IEnumerator Start()
        {
            // Skip if we're not spawning on start
            if (!SpawnOnStart) yield break;
            
            // Wait a frame for everything to be all good then start the spawn routine
            yield return null;
            yield return SpawnAsync();
        }

        /// <summary>Calling this method will spawn the object.</summary>
        public void Spawn()
        {
            StartCoroutine(SpawnAsync());
        }

        private IEnumerator SpawnAsync()
        {
            // Get the object and wait for it to load
            if (!IM.OD.TryGetValue(ObjectId, out var obj))
            {
                AtlasPlugin.Logger.LogWarning($"No object found with id '{ObjectId}'.");
                yield break;
            }
            var callback = obj.GetGameObjectAsync();
            yield return callback;
            
            // Instantiate it at our position and rotation
            Instantiate(callback.Result, transform.position, transform.rotation).SetActive(true);
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.0f, 0.0f, 0.6f, 0.5f);
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.25f);
        }
    }
}