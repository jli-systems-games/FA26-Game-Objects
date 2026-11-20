using UnityEngine;

public class Rotating : MonoBehaviour
{
    public float spinSpeed = 1080f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         transform.Rotate(0, 0, spinSpeed * Time.deltaTime);
    }
}
