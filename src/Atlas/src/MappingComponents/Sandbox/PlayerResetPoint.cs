using System.Collections;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>
    /// This component will set the scene's reset (respawn) point to it's position
    /// </summary>
    public class PlayerResetPoint : MonoBehaviour
    {
        private IEnumerator Start()
        {
            // Wait one frame so that everything is all setup
            yield return null;
            GM.CurrentSceneSettings.DeathResetPoint = transform;
        }
    }
}