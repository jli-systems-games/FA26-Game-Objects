using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}

// public class SceneSwap : MonoBehaviour
// {
 
//     //SINGLETON EXAMPLE
//     public static SceneSwap scriptInstance;//Step 1: Create a static variable for your script that you don't want destroyed on Scene switch.

  
//     private void Awake()
//     {
//         //Step 2:This if statement checks to see if an instance of this script already exists. If so, delete itself to prevent duplication.

//         if (scriptInstance == null)//If no instance of this script exists, become that instance. 
//         {

//             scriptInstance = this;
            
//         }
//         else //Otherwise, if another copy exists, delete itself on scene switch. 
//         {
//             Destroy(scriptInstance);
//         }

//         DontDestroyOnLoad(scriptInstance);//This function tells Unity to preserve this gameobject when switching scenes.

//     }
//     void Start()
//     {
//         scriptInstance = GetComponent<SceneSwap>();
//     }


//     void Update()
//     {
//         Scene currentScene = SceneManager.GetActiveScene();

//         if (Input.GetKey(KeyCode.Space)) 
//         {
//             Debug.Log("I am pressing space bar");
//             if (currentScene.name == "BlueScene") 
//             {
//                 SceneManager.LoadScene(1);
//             }
//             else if (currentScene.name == "YellowScene")
//             { 
//                 SceneManager.LoadScene(2);
//             }
//             else if (currentScene.name == "RedScene") 
//             {
//                 SceneManager.LoadScene(0);
//             }
           
//         }
//     }


// }
