using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class RaycastScript : MonoBehaviour
{
    public TMP_Text displayText;// Will display the name of the object my Raycast has detected.

    public Camera mainCamera;

    public float xMove;
    public float yMove;
    public float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        displayText = GameObject.Find("DisplayText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        //Creates a ray that shoots out from where the mouse is currently positioned on screen.
        Ray cameraRay = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit clickDetection;

        //Ray sphereRay = new Ray(transform.position, transform.forward);
        //RaycastHit raycastHit;


        if (Input.GetMouseButtonUp(0))//Activates when the player presses the right-mouse button.
        {
            if (Physics.Raycast(cameraRay, out clickDetection, Mathf.Infinity))
            {
                print("You clicked on the " + clickDetection.collider.name);
                displayText.text = clickDetection.collider.name;
            }

        }

       

        //Player Movement.
        xMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        yMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(xMove, 0, yMove);
        
    }

    public void OnTriggerEnter(Collider other)
    {
        
    }
}
