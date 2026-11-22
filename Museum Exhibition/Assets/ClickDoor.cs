using UnityEngine;

public class ClickDoor : MonoBehaviour
{
    public float openAngle = -90f;
    public float openSpeed = 10f;

    private bool isOpen = false;
    private Quaternion closedRotation;
    private Quaternion openRotation;

    void Start()
    {
        closedRotation = transform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0, openAngle, 0);
    }

    void OnMouseDown()
    {
        isOpen = !isOpen;
    }

    void Update()
    {
        Quaternion target = isOpen ? openRotation : closedRotation;

        transform.localRotation = Quaternion.Slerp(
            transform.localRotation,
            target,
            openSpeed * Time.deltaTime
        );
    }
}
