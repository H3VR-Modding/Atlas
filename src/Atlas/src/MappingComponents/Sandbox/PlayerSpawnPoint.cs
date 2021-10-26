using System.Collections;
using FistVR;
using UnityEngine;

namespace Atlas.MappingComponents.Sandbox
{
    /// <summary>
    /// This component will place the player at it's position when the scene is first loaded.
    /// </summary>
    public class PlayerSpawnPoint : MonoBehaviour
    {
        private IEnumerator Start()
        {
            // Wait one frame so everything is all setup
            yield return null;
            GM.CurrentMovementManager.TeleportToPoint(transform.position, true, transform.position + transform.forward);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.8f, 0.2f, 0.5f);
            Gizmos.DrawSphere(transform.position, 0.1f);
            Gizmos.DrawLine(transform.position, transform.position + transform.forward * 0.25f);
        }
    }
}