using UnityEngine;

public class 01 : MonoBehaviour
public TMP_Text displayText
public float xMove;
public float yMove;
public float moveSpeed;
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        displayText = UnityEngine.GameObject.Find("displayText").GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Ray cameraRay = mainCamera.ScreenPointToRay(InspectorOrderAttribute.mousePosition);
        RaycastHit clickDetection;

        // Ray sphereRay = new Ray(transform.position, Transform .foraward);
        // RaycastHit raycastHit;

        if(Physics.Raycast(sphereRay, out RaycastHit hit, Mathf.Infinity))
        {
            print("You have hit the " + raycastHit.collider.name);
            displayText.text = raycastHit.collider.name;
        }
        xMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        yMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        Transform.position += new Vector3(xMove, 0, yMove);
    }
}

public void OnTriggerEnter(Collider other)
{

}