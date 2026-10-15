using UnityEngine;
using UnityEngine.SceneManagement;

public class replay : MonoBehaviour
{
    public void ChangeScene(string scene1)
    {
        SceneManager.LoadScene("scene1");
    }

}