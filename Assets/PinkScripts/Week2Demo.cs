using UnityEngine;

public class Week2Demo : MonoBehaviour
{
    public GameObject targetObj; //Gameobject my character will move or teleport to. 
    public float moveSpeed;

    private float xMove;
    private float yMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //transform.position = targetObj.transform.position;

        //transform.position = Vector3.MoveTowards(transform.position, targetObj.transform.position, moveSpeed * Time.deltaTime);

        xMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        yMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(xMove, 0, yMove);

    }

    public void OnTriggerEnter(Collider other)
    {
        
    }

    public void OnCollisionEnter(Collision collision)
    {
        
    }

   
}
