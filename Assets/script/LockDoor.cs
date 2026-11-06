using UnityEngine;

public class LockDoor : MonoBehaviour
{
    public Vector3 openOffset = new Vector3(0, 3, 0);
    public float openSpeed = 2f;

    private bool isOpening = false;
    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if (isOpening)
        {
            transform.position = Vector3.Lerp(transform.position, startPos + openOffset, Time.deltaTime * openSpeed);
        }
    }

    public void OpenDoor()
    {
        isOpening = true;
    }
}
