using FistVR;
using Sodalite.Api;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    public class SosigSpawnPoint : MonoBehaviour
    {
        [Header("Spawn Options")] public bool SpawnOnStart = true;

        [Header("Sosig Options")] public SosigEnemyID SosigType;
        public int IFF;
        public bool SpawnActive;
        public Sosig.SosigOrder SpawnState;

        public void Start()
        {
            if (SpawnOnStart) Spawn();
        }

        public void Spawn()
        {
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