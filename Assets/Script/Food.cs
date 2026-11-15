using UnityEngine;

public class Food : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnTriggerEnter(Collider other){
        if(other.CompareTag("Player")) transform.position = new Vector3(15, 10, 0);

    }

}
