using UnityEngine;

public class FinaleTrigger : MonoBehaviour
{
    public GameObject triggeringObject;
    public GameObject finaleText;
    private bool hasFinished = false;

    void Start()
    {
        finaleText.SetActive(false);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!hasFinished && other.gameObject == triggeringObject)
        {
            finaleText.SetActive(true);
            finaleText.SetActive(true);

            Debug.Log("Wake-up machine complete!");
        }
    }
}
