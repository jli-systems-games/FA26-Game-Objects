using UnityEngine;

public class CamChanges : MonoBehaviour
{
    public GameObject cam1;
    public GameObject cam2;
    public GameObject cam3;
    public GameObject cam4;
    public GameObject cam5;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("cam1"))
        {
            cam1.SetActive(true);
        }

        if (col.gameObject.CompareTag("cam2"))
        {
            cam2.SetActive(true);
        }

        if (col.gameObject.CompareTag("cam3"))
        {
            cam3.SetActive(true);
        }

        if (col.gameObject.CompareTag("cam4"))
        {
            cam4.SetActive(true);
        }

        if (col.gameObject.CompareTag("cam5"))
        {
            cam5.SetActive(true);
        }
    }
}
