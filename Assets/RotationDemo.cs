using UnityEngine;

public class RotationDemo : MonoBehaviour
{
    public enum rotationTypes 
    { 
        Rotation,
        RotateTowards,
        RotateAround,
        TurnToObjectInRange,
        MoveCharacterWithRotation
    }
    public rotationTypes rotationState;
    private bool CanMoveCharacter = false;

    public float rotationSpeed;//Controls the speed of your rotations.
    public GameObject targetObject;//A target for your object to 'look at'. 

    private GameObject currentCam;
    private Transform originalCamPos;

   
    public float moveSpeed;
    private Vector3 horizontalMove;
    private Vector3 verticalMove;

    private float horizontalMouse;
    private float verticalMouse;
    public float sensitivity;
    private Quaternion currentRot;

    void Start()
    {
        currentCam = GameObject.Find("Main Camera");
        originalCamPos = currentCam.transform;

        currentRot = transform.localRotation;
    }

    void Update()
    {
        if (rotationState != rotationTypes.MoveCharacterWithRotation)  
        {
            CanMoveCharacter = false;
            currentCam.transform.SetParent(null);
            currentCam.transform.position = originalCamPos.position;
            currentCam.transform.rotation = originalCamPos.rotation;
        }
        switch (rotationState) 
        {
            case rotationTypes.Rotation:
                
                RotateObject();
                break;
            case rotationTypes.RotateTowards:
          
                LookAtObject();
                break;
            case rotationTypes.RotateAround:
            
                RotateAroundObject();
                break;
            case rotationTypes.MoveCharacterWithRotation:
                MoveCharacter();
                break;

        }
    }
    public void RotateObject() 
    {
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    public void RotateAroundObject() 
    {
        transform.RotateAround(targetObject.transform.position,Vector3.left, rotationSpeed * Time.deltaTime);
    }
    public void LookAtObject()
    {
        transform.LookAt(targetObject.transform.position);
    }
    public void ObjectInRange() 
    {
        //NOTE: I would recommend doing this function on the "nose" or pointer of a character and not the head itself. 
        Ray frontRay = new Ray(transform.position, transform.forward);
        RaycastHit frontRayDetect;

        //if (Physics.Raycast(frontRay, out frontRayDetect, 4 *100)) 
        //{
        //    Quaternion.Lerp(transform.rotation, targetObject.transform.position, rotationSpeed * Time.deltaTime);
        //}
    }
    public void MoveCharacter() 
    {
        CanMoveCharacter = true;
        GameObject playCamPos = GameObject.Find("PlayerCamPoint");
        currentCam.transform.position = playCamPos.transform.position;
        currentCam.transform.rotation = playCamPos.transform.rotation;

        currentCam.transform.SetParent(gameObject.transform, true);
    

        horizontalMove = Input.GetAxis("Horizontal") * transform.right;
        verticalMove = Input.GetAxis("Vertical") * new Vector3(transform.forward.x, 0, transform.forward.z);

        horizontalMouse = Input.GetAxis("Mouse Y") * sensitivity;
        verticalMouse = Input.GetAxis("Mouse X") * sensitivity;

        Vector3 playerDirection = horizontalMove + verticalMove;
        //currentRot.y += horizontalMouse;
        //currentRot.z += verticalMouse;

        transform.position += playerDirection * moveSpeed * Time.deltaTime;
        //transform.localRotation = Quaternion.Euler(0f, currentRot.y, 0f);
    }
}
