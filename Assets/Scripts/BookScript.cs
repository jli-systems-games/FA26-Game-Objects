using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BookScript : MonoBehaviour
{
    public GameObject UIPanel; // References the UI panel

    public bool toggleUI = false; // Boolean that will allow us to toggle on/off the UI panel
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [TextArea(4,5)]
    public string itemDescription;
    public TMP_Text UIPanelText;


    void Start()
    {
        UIPanelText = GameObject.Find("ItemDescription").GetComponent<TMP_Text>(); // finds game object by the name "ItemDescription" and grabs its TMP_Text component
    }

    // Update is called once per frame
    void Update()
    {   
        // If toggleUI is true, I can turn on/off the UI by pressing E
        if(toggleUI == true)
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (UIPanel.activeSelf == true)
                {
                    Debug.Log("Turn off the UI");
                    UIPanel.SetActive(false);
                }
                else if (UIPanel.activeSelf == false)
                {
                    Debug.Log("Turn on the UI");
                    UIPanel.SetActive(true);
                }
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        // If a gameobject tagged "Player" enters the book Trigger, it will toggle on the toggleUI boolean. 
         UIPanelText.text = itemDescription;
        Debug.Log("You have enetered the trigger");

        if (other.gameObject.tag == "Player")
        {
            toggleUI = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        // If the player leaves the book trigger, first it will check if the UI is still active ad turn it 
        // off. Then it will disable the players ability to toggle on/off the UI.
        UIPanelText.text = "";
        if (UIPanel.activeSelf == true)
        {
            UIPanel.SetActive(false);
        }
            toggleUI = false;
    }       
}
