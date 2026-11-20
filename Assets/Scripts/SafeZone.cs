using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class SafeZone : MonoBehaviour
{
    public static SafeZone CurrentZone;
    public static int SafeZoneCount = 0;
    public static Vector3 LastSafePoint;

    [Header("黑影在这个空间里会说的话")]
    [TextArea(1, 3)]
    public string[] whisperLines;

    [Header("黑影是否会进来游荡")]
    public bool shadowMayEnter = true;

    private BoxCollider box;

    void Awake()
    {
        box = GetComponent<BoxCollider>();
        box.isTrigger = true;
    }

    public Vector3 RandomPointInside()
    {
        Bounds b = box.bounds;
        return new Vector3(
            Random.Range(b.min.x + 1.5f, b.max.x - 1.5f),
            0f,
            Random.Range(b.min.z + 1.5f, b.max.z - 1.5f)
        );
    }

    public string RandomLine()
    {
        if (whisperLines == null || whisperLines.Length == 0) return null;
        return whisperLines[Random.Range(0, whisperLines.Length)];
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        SafeZoneCount++;
        CurrentZone = this;
        LastSafePoint = other.transform.position;
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        CurrentZone = this;
        LastSafePoint = other.transform.position;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        SafeZoneCount = Mathf.Max(0, SafeZoneCount - 1);
        if (SafeZoneCount == 0) CurrentZone = null;
    }
}