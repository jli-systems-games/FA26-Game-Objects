using UnityEngine;

public class moveScript : MonoBehaviour
{
    public float moveSpeed;
    public float xMove;
    public float yMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        yMove = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        transform.position += new Vector3(xMove, 0, yMove);
    }
}
