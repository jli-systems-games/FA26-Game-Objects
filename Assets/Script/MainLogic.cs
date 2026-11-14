using UnityEngine;
using UnityEngine.InputSystem;

public class MainLogic : MonoBehaviour
{

    public static bool key1, key2, key3, key4, key5,hasWon;
   
    public static int scene;
    
    public GameObject s1,s2,s3,s4,s5;
    private GameObject[] allScenes;

    //-1= StartUI 0 = Museum, 1 = Dave, 2 = Angel, 3 = Blasphamous, 4 = Kingdom, 5 = Hotline

    void Start()
    {

        scene = -1;

        allScenes = new[] { s1, s2, s3, s4, s5 };
        foreach (GameObject s in allScenes) s.SetActive(false);

    


    }

    void Update()
    {

        // key for testing:activate the key-----------------------
        if(Keyboard.current.digit1Key.wasPressedThisFrame) key1 = true;
        if(Keyboard.current.digit2Key.wasPressedThisFrame) key2 = true;
        if(Keyboard.current.digit3Key.wasPressedThisFrame) key3 = true;
        if(Keyboard.current.digit4Key.wasPressedThisFrame) key4 = true;
        if(Keyboard.current.digit5Key.wasPressedThisFrame) key5 = true;
        //---------------------------------------------------------  


        //go back to main for testing------------------
        if(Keyboard.current.escapeKey.wasPressedThisFrame){
            scene = 0;
            foreach (GameObject s in allScenes) s.SetActive(false);
            Debug.LogError(scene);
        } 
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
    }
}