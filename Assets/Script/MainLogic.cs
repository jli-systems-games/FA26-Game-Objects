using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainLogic : MonoBehaviour
{
    [Header("DetailPage")]
    public GameObject detailPanel;
    public Image detailImage;

    [Header("Images")]
    public Sprite leftImage;
    public Sprite middleImage;
    public Sprite rightImage;

    [Header("Buttons")]
    public Button leftButton;
    public Button middleButton;
    public Button rightButton;
    public Button escButton;

    void Start()
    {
        // hide all the details
        detailPanel.SetActive(false);
        detailImage.preserveAspect = true;  

        // Buttons's behaviour
        leftButton.onClick.AddListener(() => OpenDetail(leftImage));
        middleButton.onClick.AddListener(() => OpenDetail(middleImage));
        rightButton.onClick.AddListener(() => OpenDetail(rightImage));

        // click esc button
        escButton.onClick.AddListener(CloseDetail);
    }

    void Update()
    {
        // esc keyboard click
        if (Keyboard.current.escapeKey.wasPressedThisFrame){
            CloseDetail();
        }
    }

    void OpenDetail(Sprite artwork)     // active correspond detail image
    {
        detailImage.sprite = artwork;
        detailPanel.SetActive(true);
    }

    void CloseDetail()                 // esc function
    {
        detailPanel.SetActive(false);
    }
}