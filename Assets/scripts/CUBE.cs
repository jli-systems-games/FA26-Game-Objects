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
           // ChangeColor();
           renderer.material = myMaterial2;
        }

    }
    public void ChangeColor() 
    {
        if (renderer.material == myMaterial1)
        {
            renderer.material = myMaterial2;
        }
        else
        {
            renderer.material = myMaterial1;
        }
    }
    
}
