using UnityEngine;

public class CameraTrigger2 : MonoBehaviour
{
    public GameObject Camera1;
    public GameObject Camera2;
    public GameObject Camera3;
    private bool switched = false;    
     void Start()
    {
       Camera1.SetActive(true);
       Camera2.SetActive(false); 
       Camera3.SetActive(false);
    }
    void OnTriggerEnter(Collider other)
    {
        if (switched || !other.CompareTag("Sphere2")) return;
        Debug.Log("gulp");
        switched = true;
        Camera2.SetActive(false);
        Camera3.SetActive(true);
    }
}
