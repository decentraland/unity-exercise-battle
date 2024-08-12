using System.Collections.Generic;
using UnityEngine;

namespace Exercise.Battle.Scripts
{
    public static class Utils
    {
        public static Vector3 GetRandomPositionInBounds(Bounds bounds)
        {
            var position = Vector3.zero;
            position.x = Random.Range(bounds.min.x, bounds.max.x);
            position.z = Random.Range(bounds.min.z, bounds.max.z);
            return position;
        }

        public static Vector3 GetCenter<T>(List<T> objects) where T : Component
        {
            var center = Vector3.zero;

            foreach (var o in objects)
            {
                center += o.transform.position;
            }

            center.x /= objects.Count;
            center.y /= objects.Count;
            center.z /= objects.Count;

            return center;
        }

        public static Vector3 GetCenter(List<Transform> objects)
        {
            var center = Vector3.zero;

            foreach (var transform in objects)
            {
                center += transform.position;
            }

            center.x /= objects.Count;
            center.y /= objects.Count;
            center.z /= objects.Count;

            return center;
        }

        public static float GetNearestObject(GameObject source, List<Transform> transforms,
            out GameObject nearestObject)
        {
            var minDist = float.MaxValue;
            var sourcePosition = source.transform.position;
            nearestObject = null;

            foreach (var transform in transforms)
            {
                var dist = Vector3.Distance(sourcePosition, transform.position);
                if (!(dist < minDist)) continue;
                minDist = dist;
                nearestObject = transform.gameObject;
            }

            return minDist;
        }
    }
}