using System.Collections;
using FistVR;
using Sodalite.Api;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>
    /// Simple component to spawn a configurable Sosig.
    /// </summary>
    public class SosigSpawnPoint : MonoBehaviour
    {
        /// <summary>When true, the Sosig will be spawned on scene start.</summary>
        [Header("Spawn Options")]
        public bool SpawnOnStart = true;

        /// <summary>The type of Sosig to spawn.</summary>
        [Header("Sosig Options")]
        public SosigEnemyID SosigType;
        
        /// <summary>The IFF of the Sosig.</summary>
        public int IFF;
        
        /// <summary>The 'active' state of the Sosig. Deactivated Sosigs don't do anything.</summary>
        public bool SpawnActive;
        
        /// <summary>The initial order given to the spawned Sosig.</summary>
        public Sosig.SosigOrder SpawnState;

        public IEnumerator Start()
        {
            yield return new WaitForEndOfFrame();
            if (SpawnOnStart) Spawn();
        }

        /// <summary>Spawns the Sosig with the configured options.</summary>
        public void Spawn()
        {
            // Construct the spawn options struct
            SosigAPI.SpawnOptions options = new()
            {
                SpawnActivated = SpawnActive,
                SpawnState = SpawnState,
                IFF = IFF,
                SpawnWithFullAmmo = true,
                EquipmentMode = SosigAPI.SpawnOptions.EquipmentSlots.All,
                SosigTargetPosition = transform.position,
                SosigTargetRotation = transform.eulerAngles
            };

            // Call the Sodalite spawn method
            SosigAPI.Spawn(IM.Instance.odicSosigObjsByID[SosigType], options, transform.position, transform.rotation);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.8f, 0.2f, 0.2f, 0.5f);
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.25f);
        }
    }
}