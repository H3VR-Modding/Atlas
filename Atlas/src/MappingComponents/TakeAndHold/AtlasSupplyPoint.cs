using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.TakeAndHold
{
    public class AtlasSupplyPoint : TNH_SupplyPoint
    {
        [Header("Atlas Settings")]
        [Tooltip("If checked, this supply point will become the spawn point of every sequence if the sequences are generated.")]
        public bool ForceSpawnHere;

        [Tooltip("If checked, this supply point will only be used for the initial spawn and then never again.")]
        public bool OnlySpawn;
    }
}