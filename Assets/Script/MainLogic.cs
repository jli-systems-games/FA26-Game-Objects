using UnityEngine;
using UnityEngine.InputSystem;

public class MainLogic : MonoBehaviour
{

    public static bool key1, key2, key3, key4, key5,hasWon;
       //-1= StartUI 0 = Museum, 1 = bird, 2 = Iwanna , 3 = Blasphamous, 4 = JumpKing, 5 = Hotline
    public static int scene;

    public Camera[] cameras;
    public GameObject s1,s2,s3,s4,s5,winPPT;
    private GameObject[] allScenes;
    public bool IsTesting;



    void Start()
    {

        scene = -1;

        allScenes = new[] { s1, s2, s3, s4, s5 };
        foreach (GameObject s in allScenes) s.SetActive(false);

    


    }

    void Update()
    {

        //swtich camera
        for (int i = 0; i < cameras.Length; i++) cameras[i].enabled = (i == Mathf.Max(0, scene));  //when scene = -1 ,still maincamera(camera 0)
        if(IsTesting){
        // key for testing:activate the key-----------------------
        if(Keyboard.current.digit1Key.wasPressedThisFrame) scene =1;
        if(Keyboard.current.digit2Key.wasPressedThisFrame) scene =2;
        if(Keyboard.current.digit3Key.wasPressedThisFrame) scene =3;
        if(Keyboard.current.digit4Key.wasPressedThisFrame) scene =4;
        if(Keyboard.current.digit5Key.wasPressedThisFrame) scene =5;    
        if(Keyboard.current.digit0Key.wasPressedThisFrame) scene =0;
        }
  //---------------------------------------------------------  
        //active scene according to variable
        for (int i = 0; i < allScenes.Length; i++)
             allScenes[i].SetActive(scene == i + 1);
      
        

        //go back to main for testing------------------
        // if(Keyboard.current.escapeKey.wasPressedThisFrame){
        //     scene = 0;
        //     foreach (GameObject s in allScenes) s.SetActive(false);
        //     Debug.LogError(scene);
        // } 
        //---------------------------------------------


        //win condition
        if(key1 && key2 && key3 && key4 && key5 && !hasWon) {
            win();
            hasWon = true;
        }


    }

    void win()
    {
        Debug.LogError("u win");
        winPPT.SetActive(true);

    }
}