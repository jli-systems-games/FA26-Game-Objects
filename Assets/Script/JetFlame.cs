
using UnityEngine;

public class JetFlame : MonoBehaviour
{
    public GameObject[] flames;
    private bool onGround = false;


    void Enable(){
        foreach (GameObject f in flames) f.SetActive(false);

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground")) onGround = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground")) onGround = false;
    }

    void Update()
    {
        int n = Random.Range(0, 4);

        for (int i = 0; i < 4; i++)
            if(!onGround && i == n)flames[i].SetActive(true);
            else flames[i].SetActive(false);
    }
}
