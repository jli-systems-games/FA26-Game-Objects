using UnityEngine;

public class OldMoveScript : MonoBehaviour
{
    public float moveSpeed;
    private float horizontalMove;
    private float verticalMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove = Input.GetAxis("Horizontal");
        verticalMove = Input.GetAxis("Vertical");

        transform.position += new Vector3(horizontalMove, 0, verticalMove) * moveSpeed * Time.deltaTime;
    }
}
