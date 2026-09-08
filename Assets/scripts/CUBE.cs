using UnityEngine;
using UnityEngine.InputSystem;
public class CUBE : MonoBehaviour
{
    public float moveSpeed; 
    public Vector2 moveInput;
    public InputActionReference moveAction;

    public MeshRenderer renderer;

    public Material myMaterial1; 
    public Material myMaterial2;
    
    public bool defaultActive = true; //default state of the cube is active 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>(); 
        transform.position += new Vector3(moveInput.x, moveInput.y, 0) * moveSpeed * Time.deltaTime; 

        if (Input.GetKeyDown(KeyCode.E)) 
        {
            Debug.Log("E key was pressed"); 
            //renderer.material = myMaterial2;

            if (defaultActive)
            {
                renderer.material = myMaterial2; 
                defaultActive = false; //set the defaultActive to false so that the next time the E key is pressed, it will switch back to the original material
            }
            else
            {
                renderer.material = myMaterial1; 
                defaultActive = true; //set the defaultActive to true so that the next time the E key is pressed, it will switch back to the original material
            }
        }

    }
    
}
