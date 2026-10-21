using Unity.InferenceEngine;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;//sensitivity
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveSpeed = 5;
    }

    // Update is called once per frame
    void Update()
    {
        //player rotation
        transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X"), 0) * Time.deltaTime * rotateSpeed);

        //basic movement
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //transform.Translate(new Vector3(h, 0, v) * moveSpeed * Time.deltaTime); (world x and y)
        

        // makes sure the character moves accordeing to the direction it is moving
        Vector3 moveDirection = new Vector3(h, 0f, v);
        moveDirection = transform.TransformDirection(moveDirection);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);


    }
        
}
