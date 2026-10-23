using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    Camera cam;
    public GameObject InfoCanvas;
    public TMP_Text Name;
    public TMP_Text Description;
    
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cam = GetComponent<Camera>();

    }

    // Update is called once per frame
    void Update()
    {
        //making a ray from the canter of the viewport(camera) goes from 0-1 so 0.5
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        //hitting 
        RaycastHit hit;

        if(Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("hit something");

            if( hit.collider.gameObject.tag == "Interactive")
            {
                //do something
                Debug.Log("HitInteractive");
                InfoCanvas.SetActive(true);
                Name.text = hit.transform.gameObject.GetComponent<Interactive>().ThisItemsName;
                Description.text = hit.transform.gameObject.GetComponent<Interactive>().ThisItemsDescription;
            }
            else
            {
                Debug.Log("NotHittingInteractive");
                InfoCanvas.SetActive(false);
            }
        }
        else
        {
            Debug.Log("Hit nothing");
            InfoCanvas.SetActive(false);
        }
        //right now it works no matter how far away a thing is
       
    }

   
}

