using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    public GameObject Camera1;
    public GameObject Camera2;
    private bool switched = false;
    //im sorryy im really sorry i losr the file that we did in class so I just followed a cameratrigger tutorial
    void Start()
    {
       Camera1.SetActive(true);
       Camera2.SetActive(false); 
    }
    void OnTriggerEnter(Collider other)
    {
        if (switched || !other.CompareTag("Sphere2")) return;
        Debug.Log("hihi");
        switched = true;
        Camera1.SetActive(false);
        Camera2.SetActive(true);
    }
}
