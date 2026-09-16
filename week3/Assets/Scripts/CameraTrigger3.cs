using UnityEngine;

public class CameraTrigger3 : MonoBehaviour
{
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    public GameObject Camera4;
    private bool switched = false;    
     void Start()
    {
       Camera1.SetActive(true);
       Camera2.SetActive(false); 
       Camera3.SetActive(false);
       Camera4.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (switched || !other.CompareTag("Sphere2")) return;
        Debug.Log("This is the last camera");
        switched = true;
        Camera3.SetActive(false);
        Camera4.SetActive(true);
    }
}
