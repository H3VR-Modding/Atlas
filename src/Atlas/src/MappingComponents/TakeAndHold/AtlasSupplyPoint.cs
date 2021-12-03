using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Atlas.MappingComponents.TakeAndHold
{
    public class AtlasSupplyPoint : MonoBehaviour
    {
        public Transform Bounds = null!;
        
        [Header("Spawn Points")]
        public Transform Player = null!;
        public List<Transform> SosigDefense = null!;
        public List<Transform> Turrets = null!;
        public List<Transform> Panels = null!;
        public List<Transform> Boxes = null!;
        public List<Transform> Tables = null!;
        public Transform CaseLarge = null!;
        public Transform CaseSmall = null!;
        public Transform Melee = null!;
        public List<Transform> SmallItem = null!;
        public Transform Shield = null!;
        
        [Header("Atlas Settings")]
        [Tooltip("If checked, this supply point will become the spawn point of every sequence if the sequences are generated.")]
        public bool ForceSpawnHere;

        [Tooltip("If checked, this supply point will only be used for the initial spawn and then never again.")]
        public bool OnlySpawn;

        private void OnDrawGizmos()
        {
            Extensions.GenericGizmoCubeOutline(Color.white, Vector3.zero, Vector3.one, Bounds);
            Extensions.GenericGizmoCube(new Color(0.4f, 0.4f, 0.9f, 0.5f), new Vector3(0f, 1.5f, 0.25f), new Vector3(2.3f, 1.2f, 0.5f), Vector3.forward, Panels.ToArray());
            Extensions.GenericGizmoCube(new Color(1f, 0.4f, 0.0f, 0.5f), new Vector3(0f, 0.12f, 0f), new Vector3(1.4f, 0.24f, 0.35f), Vector3.forward, CaseLarge);
            Extensions.GenericGizmoCube(new Color(1f, 0.4f, 0.0f, 0.5f), new Vector3(0f, 0.12f, 0f), new Vector3(0.6f, 0.24f, 0.35f), Vector3.forward, CaseSmall);
            Extensions.GenericGizmoCube(new Color(0.5f, 0.5f, 0.5f, 0.5f), new Vector3(0f, 0.4f, 0.1f), new Vector3(0.7f, 0.8f, 1.5f), Vector3.zero, Tables.ToArray());
            Extensions.GenericGizmoCube(new Color(0.7f, 0.7f, 0.7f, 0.5f), Vector3.zero, 0.5f * Vector3.one, Vector3.zero, Boxes.ToArray());
            Extensions.GenericGizmoSphere(new Color(0.8f, 0f, 0f, 0.5f), Vector3.zero, 0.25f, SosigDefense.ToArray());
            Extensions.GenericGizmoSphere(new Color(0.8f, 0f, 0f, 0.1f), Vector3.zero, 0.25f, Turrets.AsEnumerable().ToArray());
            Extensions.GenericGizmoSphere(new Color(0.0f, 0.8f, 0f, 0.5f), Vector3.zero, 0.2f, Melee);
            Extensions.GenericGizmoSphere(new Color(0.0f, 0.8f, 0f, 0.5f), Vector3.zero, 0.1f, SmallItem.ToArray());
            Extensions.GenericGizmoSphere(new Color(0.0f, 0.8f, 0.8f, 0.5f), Vector3.zero, 0.25f, Player);
            Extensions.GenericGizmoCube(new Color(0.0f, 0.8f, 0.0f, 0.1f), Vector3.zero, new Vector3(0.4f, 0.6f, 0.1f), Vector3.zero, Shield);
        }
    }
}