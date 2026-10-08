using UnityEngine;
using UnityEngine.SceneManagement;

public class scenechange : MonoBehaviour
{
    public void LoadScene(string scene2)
    {
        SceneManager.LoadScene(scene2);
    }
}
