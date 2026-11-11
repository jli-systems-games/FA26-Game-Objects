using UnityEngine;

public class PlayerRotate : MonoBehaviour
{
    //private Vector2 horizontalMouse;
    //private Vector2 verticalMouse;
    private Vector2 headTurn;
    public float sensitivity;
    private Quaternion currentRot;

    public float maxRot, minRot;
    //public float minRot;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //headTurn = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        headTurn.y += Input.GetAxis("Mouse Y") * sensitivity;
        headTurn.x += Input.GetAxis("Mouse X") * sensitivity;

        //horizontalMouse.y -= Mathf.Clamp(horizontalMouse.y, minRot, maxRot);
        //verticalMouse.x += Mathf.Clamp(verticalMouse.x, minRot, maxRot);

        //currentRot = Quaternion.Euler(-currentRot.y, verticalMouse.x, 0);
       
    }
}
