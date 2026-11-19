using UnityEngine;

public class TriggerBlendOut : MonoBehaviour
{
    public GameObject blackScreen;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            blackScreen.SetActive(false);
            blackScreen.SetActive(true);
        }
    }
}