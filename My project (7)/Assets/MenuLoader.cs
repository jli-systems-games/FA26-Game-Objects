using UnityEngine;
using UnityEngine.SceneManagement;

public class ReloadScene : MonoBehaviour
{
    public GameObject menuPanel; //拖入你的菜单Panel

    public void StartGame()
    {
        menuPanel.SetActive(false); //隐藏菜单面板，菜单画面消失
    }
}
