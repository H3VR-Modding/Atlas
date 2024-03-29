﻿using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.TakeAndHold
{
    public class AtlasHoldPoint : MonoBehaviour
    {
        public List<Transform> Bounds = null!;
        public GameObject NavBlockers = null!;
        public List<Transform> BarrierPoints = null!;
        public List<TNH_HoldPoint.AttackVector> AttackVectors = null!;
        
        [Header("Spawn Points")]
        public Transform SystemNode = null!;
        public List<Transform> Targets = null!;
        public List<Transform> Turrets = null!;
        public List<Transform> SosigDefense = null!;
        
        private void OnDrawGizmos()
        {
            if (SystemNode) Extensions.GenericGizmoSphere(new Color(0.0f, 0.8f, 0.8f), 1.5f * Vector3.up, 0.25f, SystemNode);
            
            Extensions.GenericGizmoCubeOutline(Color.white, Vector3.zero, Vector3.one, Bounds.ToArray());
            Extensions.GenericGizmoSphere(new Color(0.8f, 0f, 0f, 0.5f), Vector3.zero, 0.25f, SosigDefense.ToArray());
            Extensions.GenericGizmoSphere(new Color(0.8f, 0f, 0f, 0.1f), Vector3.zero, 0.25f, Turrets.ToArray());
            Extensions.GenericGizmoCube(new Color(1f, 0.0f, 0.0f, 0.5f), Vector3.zero, new Vector3(0.1f, 0.5f, 0.1f), Vector3.zero, Targets.ToArray());
            Extensions.GenericGizmoCube(new Color(0.0f, 0.6f, 0.0f, 0.5f), new Vector3(0, 1, 0), new Vector3(1f, 2, 0.1f), Vector3.forward, BarrierPoints.ToArray());

            foreach (var attackVector in AttackVectors)
            {
                Extensions.GenericGizmoSphere(new Color(0.8f, 0f, 0f, 0.5f), Vector3.zero, 0.25f, attackVector.SpawnPoints_Sosigs_Attack.ToArray());
                if (attackVector.GrenadeVector) Extensions.GenericGizmoCube(new Color(0.1f, 0.5f, 0.1f, 0.5f), Vector3.zero, 0.5f * Vector3.one, Vector3.forward, attackVector.GrenadeVector);
            }
        }
    }
}