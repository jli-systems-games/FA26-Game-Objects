using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    Camera cam;
    public GameObject InfoCanvas;
    public TMP_Text Name;
    public TMP_Text Description;

    public GameObject PressE;
    public GameObject GetCloser;
    public GameObject NotInteractable;

    public bool infoShowing = false;
    
    

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
        RaycastHit hit; //make variable

        if(Physics.Raycast(ray, out hit, 100))
        {
            Debug.Log("hit something");

           // if(hit.distance > 1f && hit.collider.gameObject.tag == "Interactive")
            //{
            //    //do something
            //    Debug.Log("HitInteractive");
            //    GetCloser.SetActive(false);
            //    NotInteractable.SetActive(false);
            //    PressE.SetActive(true);
                
            //}
            if(Input.GetKeyDown(KeyCode.E) && hit.distance > 1f && hit.collider.gameObject.tag == "Interactive")
            {
                Debug.Log("Hit interactive far");
                NotInteractable.SetActive(false);
                PressE.SetActive(false);
                GetCloser.SetActive(true);
            }
            if(Input.GetKeyDown(KeyCode.E) && hit.collider.gameObject.tag != "Interactive")
            {
                Debug.Log("Hit notinteractive");
                PressE.SetActive(false);
                GetCloser.SetActive(false);
                NotInteractable.SetActive(true);
            }
            if (Input.GetKeyDown(KeyCode.E) && hit.collider.gameObject.tag == "Interactive" && hit.distance < 1f) 
            {
                PressE.SetActive(false);
                GetCloser.SetActive(false);
                NotInteractable.SetActive(false);
                InfoCanvas.SetActive(true);
                Name.text = hit.transform.gameObject.GetComponent<Interactive>().ThisItemsName;
                Description.text = hit.transform.gameObject.GetComponent<Interactive>().ThisItemsDescription;
                infoShowing = true;
            }
            else if (Input.GetKeyDown(KeyCode.E) && infoShowing == true)
            {
                Debug.Log("NotHittingInteractive");
                InfoCanvas.SetActive(false);
            }
            else if (hit.collider.gameObject.tag != "Interactive" && infoShowing == true)
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

