using UnityEngine;

public class Camera : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    // Update is called once per frame
    void Update()
    {
        if(Timer.timeLeft<=0){
            transform.position = new Vector3(14.5f,1f,-2f);
            transform.eulerAngles = new Vector3(11f, 0f, 0f);
        } 
    }
}
