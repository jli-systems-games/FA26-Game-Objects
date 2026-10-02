using Unity.VisualScripting;
using UnityEngine;

public class CanvasSwap : MonoBehaviour
{
    public GameObject selfCanvasRef;//ref to the canvas that is on, self
    public GameObject newCanvas;//the one we swap to

    //public GameObject currentCamera;//this keeps track of what the current camera is
    
    void Start()
    {
        selfCanvasRef = this.gameObject;
        //currentCamera = mainCameraRef;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void switchCanvas() {

        Debug.Log("I Have Been Hit");

        //currentCamera.SetActive(false);\
        newCanvas.SetActive(true);
        selfCanvasRef.SetActive(false);
        //currentCamera = switchCamera;

    }
}
