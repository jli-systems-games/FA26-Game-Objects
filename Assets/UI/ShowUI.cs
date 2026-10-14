using UnityEngine;
using UnityEngine.SceneManagement; 

public class ShowUI : MonoBehaviour
{

    public GameObject canvasOn;
    public Player playerScriptRef;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerScriptRef.Whateverfunction(); whatevere happen in tht function would happen here
    }

    // Update is called once per frame
    void Update()
    {
        if (playerScriptRef.health == 0) 
        {
           
            canvasOn.SetActive(true);
        }
    }
    public void restartGame()
    { 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void endGame()
    {
 
        Application.Quit();

        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
