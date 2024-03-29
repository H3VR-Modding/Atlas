﻿using System;
using System.Collections;
using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>This component will spawn a functional prefab at it's position</summary>
    /// <remarks>Using this component will destroy the game object it's placed on when it spawns the prefab!</remarks>
    public class PrefabSpawnPoint : MonoBehaviour
    {
        internal static readonly Dictionary<PrefabType, GameObject> CachedObjects = new();
        internal static bool ObjectsCached = false;

        /// <summary>The available prefabs for spawning</summary>
        public enum PrefabType
        {
            /// <summary>Item spawner</summary>
            ItemSpawner,

            /// <summary>Garbage bin that destroys objects</summary>
            Destructobin,
            
            /// <summary>Utility tool that lets you spawn Sosigs.</summary>
            SosigSpawner,
            
            /// <summary>Crafting machine used in RotR.</summary>
            WhizzBangADinger,
            
            /// <summary>Remote detonator tool used for some items crafted with the <see cref="WhizzBangADinger"/>.</summary>
            BangerDetonator
        }

        /// <summary>The type of prefab to spawn.</summary>
        public PrefabType Type;

        private IEnumerator Start()
        {
            // Wait until the objects are cached
            while (!ObjectsCached) yield return null;

            // Adjust the spawn position slightly if necessary.
            Vector3 spawnPosition = transform.position;
            
            // Do some special stuff for the item spawner
            if (Type == PrefabType.ItemSpawner)
            {
                // It needs to come forward a bit
                spawnPosition += transform.forward * 0.025f;
            }
            
            // Make a copy of the object we want to spawn and set it active
            // If it's null or missing then just ignore
            if (CachedObjects.ContainsKey(Type) && CachedObjects[Type])
            {
                GameObject obj = Instantiate(CachedObjects[Type], spawnPosition, transform.rotation);
                obj.SetActive(true);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.4f, 0.4f, 0.9f, 0.5f);
            Gizmos.matrix = transform.localToWorldMatrix;
            Vector3 center, size, forward;
            switch (Type)
            {
                case PrefabType.ItemSpawner:
                    center = new Vector3(0f, 0.7f, 0.25f);
                    size = new Vector3(2.3f, 1.2f, 0.5f);
                    forward = Vector3.forward;
                    break;
                case PrefabType.Destructobin:
                    center = new Vector3(0f, 0.525f, 0f);
                    size = new Vector3(0.65f, 1.05f, 0.65f);
                    forward = Vector3.left;
                    break;
                case PrefabType.SosigSpawner:
                    center = new Vector3(0f, 0.11f, 0f);
                    size = new Vector3(0.3f, 0.43f, 0.05f);
                    forward = Vector3.forward;
                    break;
                case PrefabType.WhizzBangADinger:
                    center = new Vector3(0f, 0.425f, -0.05f);
                    size = new Vector3(0.4f, 0.85f, 0.5f);
                    forward = Vector3.forward;
                    break;
                case PrefabType.BangerDetonator:
                    center = new Vector3(0f, 0f, 0.1f);
                    size = new Vector3(0.05f, 0.05f, 0.33f);
                    forward = Vector3.forward;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Gizmos.DrawCube(center, size);
            Gizmos.DrawLine(center, center + forward * 0.5f);
        }
    }
}
