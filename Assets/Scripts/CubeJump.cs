using UnityEngine;
public class Cubejump : MonoBehaviour
{
    public Rigidbody rb;
    private bool onGround;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.linearVelocity.y >= 0.1f)
        {
            onGround = false;
        }
        else
        {
            onGround = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && onGround == true)
        {
            rb.AddForce(Vector3.up);
        }
    }
}
