using UnityEngine;

public class JellyfishMov : MonoBehaviour

{
    public float speed; 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.S)) {
            transform.position += new Vector3(0, -speed, 0) * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.D)) {
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        
    }
}
