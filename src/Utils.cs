using UnityEngine;

namespace LessFlashingLights;

public static class Utils
{
    public static GameObject Child(this GameObject go, string name)
    {
        return go.transform.Find(name).gameObject;
    }
}