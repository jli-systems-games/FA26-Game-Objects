using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCanvasScript : MonoBehaviour
{
    public GameObject SettingsCanvas;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SettingsCanvas = GameObject.Find("SettingsCanvas");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Quit()
    {
        Application.Quit();
#if UNITY_EDITOR
        // This stops play mode if you are running inside the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void Settings()
    {
        SettingsCanvas.SetActive(true);

    }
  //scriptimfindingvar = GameObject.Find("Object its in").GetComponent<nameofscript>();

}
