using UnityEngine;

public sealed class ApartmentShellSettings : MonoBehaviour
{
    [Min(0.0001f)] public float unityUnitsPerPlanPixel = 0.01f;
    [Min(0.01f)] public float wallHeight = 2.7f;
}
