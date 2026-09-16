using UnityEngine;

public class StartContraption : MonoBehaviour
{
    public Rigidbody startBall;
    void Start()
    {
        startBall.isKinematic = true;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            startBall.isKinematic = false;
            Debug.Log("Contraption started!");
        }

        
    }
}
