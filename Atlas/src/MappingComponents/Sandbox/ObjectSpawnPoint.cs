﻿using System.Collections;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>
    /// Spawn point component for an Anvil object
    /// </summary>
    public class ObjectSpawnPoint : MonoBehaviour
    {
        public string ObjectId = "";
        public bool SpawnOnStart = true;

        private IEnumerator Start()
        {
            // Skip if we're not spawning on start
            if (!SpawnOnStart) yield break;
            
            // Wait a frame for everything to be all good then start the spawn routine
            yield return null;
            yield return SpawnAsync();
        }

        public void Spawn()
        {
            StartCoroutine(SpawnAsync());
        }

        private IEnumerator SpawnAsync()
        {
            // Get the object and wait for it to load
            FVRObject obj = IM.OD[ObjectId];
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