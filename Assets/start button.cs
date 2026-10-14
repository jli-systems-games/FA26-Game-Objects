using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeScene(string scene2) 
    {
        SceneManager.LoadScene("scene2");
    }

}