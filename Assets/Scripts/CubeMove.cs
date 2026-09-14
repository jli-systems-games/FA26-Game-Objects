using System.Numerics;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMove : MonoBehaviour
{
    public GameObject Midorisan;
    /// <summary>
    public GameObject Midorichan;//gugugaga bananapatoopooy
    /// </summary>
    public float moveSpeed;
   
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position += transform.forward * moveSpeed * Time.deltaTime;
        //horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        //verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //transform.position += new UnityEngine.Vector3(horizontalMove, 0, verticalMove);
        // moveInput = moveAction.action.ReadValue<UnityEngine.Vector2>();
        // transform.position += new UnityEngine.Vector3(moveInput.x, 0, moveInput.y) * moveSpeed * Time.deltaTime;
        //my input map somehow doesnt work anymore so I followed a tutorial so that the cube can move by pressing awsd I am really really sorry for this...
        UnityEngine.Vector3 movement = UnityEngine.Vector3.zero;
        //I also don't know why, this is something I noticed in class that I have to add unityengine before vector3 or it would say "vector3" is an ambiguous reference betweem 'UnityEngine.Vector3' and 'System.Numerics.Vector3'
        if (Keyboard.current != null)
        {
            if (Keyboard.current.wKey.isPressed)
                movement += transform.forward;

            if (Keyboard.current.sKey.isPressed)
                movement -= transform.forward;

            if (Keyboard.current.aKey.isPressed)
                movement -= transform.right;

            if (Keyboard.current.dKey.isPressed)
                movement += transform.right;
        }
         transform.position += movement.normalized * moveSpeed * Time.deltaTime;
        //my brain is growing so movement.normalized is to keep the distance moved by keyboard not stack on each other, like when i press d and w at the same time the direction changes while the distance moved does not pile up just because I pressed two keys at the same ime
        //hope this works on my cube
        //ok it worked but seems like the cube only moves basd on the directions it is facing instead of the camera angle, for now I will just change the camera position to align the cube
    }
}
