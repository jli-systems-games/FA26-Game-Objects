using UnityEngine;

public class RandomScene : MonoBehaviour

{
    public GameObject mainScreen;
    public GameObject[] scenes;
    private GameObject currentScene;


    void Start()
    {
        mainScreen.SetActive(true);

        foreach (GameObject scene in scenes)
        {
            scene.SetActive(false);
        }

        currentScene = mainScreen;
    }

    public void RandomCamera()
    {
        if (currentScene != null)
        {
            currentScene.SetActive(false);
        }
        int randomIndex = Random.Range(0, scenes.Length);
        currentScene = scenes[randomIndex];
        currentScene.SetActive(true);
    }
}
