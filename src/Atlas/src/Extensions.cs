using System.Collections.Generic;
using UnityEngine;

namespace Atlas
{
    public static class Extensions
    {
        public static IEnumerable<GameObject> IterateChildren(this GameObject go)
        {
            for (var i = 0; i < go.transform.childCount; i++)
                yield return go.transform.GetChild(i).gameObject;
        }

        public static IEnumerable<Transform> IterateChildren(this Transform go)
        {
            for (var i = 0; i < go.transform.childCount; i++)
                yield return go.transform.GetChild(i);
        }

        public static void GenericGizmoCube(Color color, Vector3 center, Vector3 size, Vector3 forward,
            params Transform[] markers)
        {
            Gizmos.color = color;
            foreach (Transform ii in markers)
            {
                Gizmos.matrix = ii.localToWorldMatrix;
                Gizmos.DrawCube(center, size);
                Gizmos.DrawLine(center, center + forward);
            }
        }

        public static void GenericGizmoCubeOutline(Color color, Vector3 center, Vector3 size,
            params Transform[] markers)
        {
            Gizmos.color = color;
            foreach (Transform ii in markers)
            {
                Gizmos.matrix = ii.localToWorldMatrix;
                Gizmos.DrawWireCube(center, size);
            }
        }

        public static void GenericGizmoSphere(Color color, Vector3 center, float radius, params Transform[] markers)
        {
            Gizmos.color = color;
            foreach (Transform ii in markers)
            {
                Gizmos.matrix = ii.localToWorldMatrix;
                Gizmos.DrawSphere(center, radius);
            }
        }
    }
}