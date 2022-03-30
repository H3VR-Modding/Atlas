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
            // Wait until next frame so that everything is all setup
            yield return null;

            // If the death point is already set (which it should be) move it to here.
            // Otherwise set this as the reset point.
            var deathResetPoint = GM.CurrentSceneSettings.DeathResetPoint;
            if (deathResetPoint) deathResetPoint.SetPositionAndRotation(transform.position, transform.rotation);
            else GM.CurrentSceneSettings.DeathResetPoint = transform;
        }
    }
}