using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents
{
    /// <summary>
    /// Scene overrides for the Reverb System
    /// </summary>
    public class ReverbSystemOverride : MonoBehaviour
    {
        public List<FVRReverbEnvironment> Environments = new();
        public int NumToCheckAFrame = 13;
        public FVRReverbEnvironment? DefaultEnvironment;
        
        internal void ApplyOverride(FVRReverbSystem reverbSystem)
        {
            // Apply overrides
            reverbSystem.Environments = Environments;
            reverbSystem.NumToCheckAFrame = NumToCheckAFrame;
            reverbSystem.CurrentReverbEnvironment = DefaultEnvironment;
            reverbSystem.DefaultEnvironment = DefaultEnvironment;
            
            // Delete the reverb environment that comes with the scene
            Destroy(reverbSystem.transform.GetChild(0).gameObject);
        }
    }
}