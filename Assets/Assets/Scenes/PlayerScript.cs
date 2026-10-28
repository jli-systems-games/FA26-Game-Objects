
using UnityEngine;


public class PlayerScript : MonoBehaviour

{
    public float moveSpeed;
    private float HorizontalMove;
    private float VerticalMove;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        HorizontalMove = Input.GetAxis("Horizontal");
        VerticalMove = Input.GetAxis("Vertical");
        transform.position += new Vector3(HorizontalMove, 0, VerticalMove) * moveSpeed * Time.deltaTime;

    }
}
