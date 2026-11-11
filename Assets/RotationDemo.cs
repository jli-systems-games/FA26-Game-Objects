using UnityEngine;

public class RotationDemo : MonoBehaviour
{
    //THIS SCRIPT CONTAINS A FEW EXAMPLES OF COMMON FUNCTIONS TO ROTATE OBJECTS. ATTACH THIS TO A GAMEOBJECT
    //YOU WISH TO ROTATE AND SELECT WHICH ONE YOU HAVE ACTIVE IN THE DROP DOWN LIST.
    public enum rotationTypes 
    { 
        Rotation,
        RotateTowards,
        RotateAround
   
    }
    [Header("Use this drop down menu to toggle different Rotation effects.")]
    public rotationTypes rotationState;

    [Header("Rotation controls the speed and Target Object controls where the object look towards.")]
    public float rotationSpeed;//Controls the speed of your rotations.
    public GameObject targetObject;//A target for your object to 'look at'. 

    //THE VARIABLES BELOW IS RELATED TO MOVEMENT. IT HAS NO EFFECT ON THE ROTATION EXAMPLES. 
    //I WOULD NOT RECOMMEND ATTACHING ROTATION AND MOVEMENT WITHIN THE SAME SCRIPT. 
    private Vector3 horizontalMove;
    private Vector3 verticalMove;
    public float moveSpeed;

    

    void Start()
    {

    }

    void Update()
    {
       //For easy swapping, I use enums to swap between the different rotation types. 
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
          
        }

        //CODE FOR MOVEMENT. NOT RELEVANT TO THE ROTATION EXAMPLES.
        horizontalMove = Input.GetAxis("Horizontal") * transform.right;
        verticalMove = Input.GetAxis("Vertical") * new Vector3(transform.forward.x, 0, transform.forward.z);

        Vector3 playerDirection = horizontalMove + verticalMove;
        transform.position += playerDirection * moveSpeed * Time.deltaTime;
    }
    public void RotateObject() 
    {
        //Rotates an object on a specified axis. The order is 'x', 'y', 'z'.
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }

    public void RotateAroundObject() 
    {
        //Rotates the object around another another target gameobject. Control the direction of the rotation by modifying the 
        // 'Vector3.left' portion of the function.
        transform.RotateAround(targetObject.transform.position,Vector3.left, rotationSpeed * Time.deltaTime);
    }
    public void LookAtObject()
    {
        //The object looks towards another target gameobject.
        transform.LookAt(targetObject.transform.position);
    }
   
}
