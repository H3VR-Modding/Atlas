using System.Collections.Generic;
using UnityEngine;

namespace Atlas
{
    /// <summary>
    /// Collection of useful extensions that can be used for various arbitrary things.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Iterate over the children of a game object as if it were a collection
        /// </summary>
        public static IEnumerable<GameObject> IterateChildren(this GameObject go)
        {
            for (var i = 0; i < go.transform.childCount; i++)
                yield return go.transform.GetChild(i).gameObject;
        }

        /// <summary>
        /// Iterate over the children of a transform as if it were a collection
        /// </summary>
        public static IEnumerable<Transform> IterateChildren(this Transform go)
        {
            for (var i = 0; i < go.transform.childCount; i++)
                yield return go.transform.GetChild(i);
        }

        /// <summary>
        /// Draw a collection of gizmo cubes with the same color, size, center, and rotation
        /// </summary>
        public static void GenericGizmoCube(Color color, Vector3 center, Vector3 size, Vector3 forward, params Transform[] markers)
        {
            Gizmos.color = color;
            foreach (Transform ii in markers)
            {
                if (!ii) continue;

                Gizmos.matrix = ii.localToWorldMatrix;
                Gizmos.DrawCube(center, size);
                Gizmos.DrawLine(center, center + forward);
            }
        }

        /// <summary>
        /// Draw a collection of outline gizmo cubes with the same color, size, center, and rotation
        /// </summary>
        public static void GenericGizmoCubeOutline(Color color, Vector3 center, Vector3 size, params Transform[] markers)
        {
            Gizmos.color = color;
            foreach (Transform ii in markers)
            {
                if (!ii) continue;

                Gizmos.matrix = ii.localToWorldMatrix;
                Gizmos.DrawWireCube(center, size);
            }
        }

        /// <summary>
        /// Draw a collection of gizmo spheres with the same color, center, and radius
        /// </summary>
        public static void GenericGizmoSphere(Color color, Vector3 center, float radius, params Transform[] markers)
        {
            Gizmos.color = color;
            foreach (Transform ii in markers)
            {
                if (!ii) continue;

                Gizmos.matrix = ii.localToWorldMatrix;
                Gizmos.DrawSphere(center, radius);
            }
        }
    }
}