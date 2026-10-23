using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MainCanvas;
    public bool MainCanvasOn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MainCanvasOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (MainCanvasOn == false)
            {
                Debug.Log("canvasOn");
                MainCanvas.SetActive(true);
                MainCanvasOn = true;
            }
            else
            {
                Debug.Log("canvasOff");
                MainCanvas.SetActive(false);
                MainCanvasOn = false;
            }
        }
       
    }

    
}
