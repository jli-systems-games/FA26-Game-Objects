using UnityEngine;
using UnityEngine.InputSystem;

public class GalleryDoorInteraction : MonoBehaviour
{
    public Transform door;
    public float interactionDistance = 3.2f;
    public float openAngle = 95f;

    private bool isOpen;
    private Quaternion closedRotation;
    private Quaternion targetRotation;

    private void Start()
    {
        if (door == null) door = transform;
        closedRotation = door.localRotation;
        targetRotation = closedRotation;
    }

    private void Update()
    {
        var camera = Camera.main;
        if (camera == null) return;

        if (IsLookedAt(camera) && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            isOpen = !isOpen;
            float direction = name.Contains("Port") ? -1f : 1f;
            targetRotation = closedRotation * Quaternion.Euler(0, isOpen ? direction * openAngle : 0, 0);
        }
        door.localRotation = Quaternion.Slerp(door.localRotation, targetRotation, Time.deltaTime * 4f);
    }

    private bool IsLookedAt(Camera camera)
    {
        if (Vector3.Distance(camera.transform.position, transform.position + Vector3.up * 1.5f) > interactionDistance) return false;
        if (!Physics.Raycast(camera.transform.position, camera.transform.forward, out var hit, interactionDistance)) return false;
        return hit.collider.GetComponentInParent<GalleryDoorInteraction>() == this;
    }

    private void OnGUI()
    {
        var camera = Camera.main;
        if (camera == null || !IsLookedAt(camera)) return;
        var label = isOpen ? "Press E to close door" : "Press E to open door";
        var style = new GUIStyle(GUI.skin.box) { fontSize = 18, alignment = TextAnchor.MiddleCenter };
        GUI.Box(new Rect(Screen.width * .5f - 120, Screen.height - 88, 240, 42), label, style);
    }
}
