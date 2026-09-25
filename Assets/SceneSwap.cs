using System.Reflection.Metadata;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwap : MonoBehaviour
{
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
      
    }

    // Update is called once per frame
    void Update()
    {
        Scene currentScene = SceneManager.GetActiveScene();

        if (Input.GetKey(KeyCode.Space)) 
        {
            if (currentScene.name == "BlueScene") 
            {
                SceneManager.LoadScene(1);
            }
            else if (currentScene.name == "YellowScene")
            { 
                SceneManager.LoadScene(2);
            }
            else if (currentScene.name == "RedScene") 
            {
                SceneManager.LoadScene(0);
            }
           
        }
    }

    public void Whatever() 
    { 
     
    }
}
