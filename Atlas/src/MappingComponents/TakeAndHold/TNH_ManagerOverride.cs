using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.TakeAndHold
{
    /// <summary>
    /// Use this component to override values on the TNH_Manager
    /// </summary>
    public class TNH_ManagerOverride : MonoBehaviour
    {
        [Header("Game State")] public bool IsBigLevel = false;
        public bool UsesClassicPatrolBehaviour = true;

        [Header("Data Settings")] public TNH_PointSequence[]? PossibleSequences;
        public TNH_SafePositionMatrix? SafePosMatrix;

        [Header("System Connections")] public TNH_HoldPoint[]? HoldPoints;
        public TNH_SupplyPoint[]? SupplyPoints;

        internal void ApplyOverrides(TNH_Manager manager)
        {
            
        }
    }
}