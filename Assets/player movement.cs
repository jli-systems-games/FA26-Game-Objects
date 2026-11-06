using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float moveSpeed;
    private float horizontalMove;
    private float verticalMove;
    
    void Start()
    {

    }

   
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        verticalMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(horizontalMove, 0, verticalMove) ;
    }
}