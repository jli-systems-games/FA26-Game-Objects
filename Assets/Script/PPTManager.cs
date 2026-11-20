
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class PPTManager : MonoBehaviour
{
    [System.Serializable]
    public class Page
    {
        public Sprite image;
        [TextArea]
        public string description;
    }

    public Page[] pages;
    public Image picture;
    public TMP_Text text;

    private int currentPage = 0;

    void OnEnable()
    {
        currentPage = 0;
        ShowPage();
        Time.timeScale = 0f;
    }

    void OnDisable(){
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame ||
            Keyboard.current.spaceKey.wasPressedThisFrame ||
            Keyboard.current.enterKey.wasPressedThisFrame)
        {
            currentPage++;

            if (currentPage >= pages.Length){
                Time.timeScale = 1f;
                Destroy(gameObject);
            }
                
            else
                ShowPage();
        }
    }

    void ShowPage()
    {
        if (picture != null)
        {
            picture.sprite = pages[currentPage].image;
            picture.enabled = pages[currentPage].image != null;
        }

        text.text = pages[currentPage].description;
    }
}
