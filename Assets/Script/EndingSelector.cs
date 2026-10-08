using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSelector : MonoBehaviour
{
    public int End2Unlock = 3; // To unlock End2, the player must have a count of 3

    public void GoToEnding()
    {
        if (GameManager.instance.count >= End2Unlock)
        {
            SceneManager.LoadScene("End2");
        }
        else
        {
            SceneManager.LoadScene("End1");
        }
    }
}
