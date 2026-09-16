using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    public GameObject Congrats;

    

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Sphere2"))
        {
            Congrats.SetActive(true);
        }        
    }
}
