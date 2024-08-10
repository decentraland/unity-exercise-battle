using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
    public static Vector3 GetRandomPosInBounds(Bounds bounds)
    {
        var pos = Vector3.zero;
        pos.x = Random.Range(bounds.min.x, bounds.max.x);
        pos.z = Random.Range(bounds.min.z, bounds.max.z);
        return pos;
    }

    public static Vector3 GetCenter<T>(List<T> objects)
        where T : Component
    {
        var result = Vector3.zero;

        foreach (var o in objects)
        {
            result += o.transform.position;
        }

        result.x /= objects.Count;
        result.y /= objects.Count;
        result.z /= objects.Count;

        return result;
    }

    public static Vector3 GetCenter(List<GameObject> objects)
    {
        var result = Vector3.zero;

        foreach (var o in objects)
        {
            result += o.transform.position;
        }

        result.x /= objects.Count;
        result.y /= objects.Count;
        result.z /= objects.Count;

        return result;
    }

    public static float GetNearestObject(GameObject source, List<GameObject> objects, out GameObject nearestObject)
    {
        var minDist = float.MaxValue;
        nearestObject = null;

        foreach (var obj in objects)
        {
            var dist = Vector3.Distance(source.transform.position, obj.transform.position);
            if (!(dist < minDist)) continue;
            minDist = dist;
            nearestObject = obj;
        }

        return minDist;
    }
}