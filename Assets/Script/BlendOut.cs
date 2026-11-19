using UnityEngine;
using UnityEngine.UI;
public class BlendOut : MonoBehaviour
{

    public float fadeTime = 1f;
    private Image image;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }


    void OnEnable()
    {
        image = GetComponent<Image>();
        image.color = Color.black;
    }

    void Update()
    {
        Color changeColor = image.color;
        changeColor.a = Mathf.Max(0, changeColor.a - Time.unscaledDeltaTime / fadeTime);
        image.color = changeColor;

        if (changeColor.a <= 0) gameObject.SetActive(false);
    }
}
