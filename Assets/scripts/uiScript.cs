using Unity.Mathematics;
using UnityEngine;

public class uiScript : MonoBehaviour
{

    public CanvasGroup panel;
    public GameObject weapon;
    public float fadeSpeed = 4f;
    float showing = 0f;//0 hidden, 1 showing
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       panel.alpha = 0f; 
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (weapon != null && weapon.activeInHierarchy)
            {
                showing = 1f;
            }
            else
            {
                showing = 0f;
            }
        }
    }
    // Update is called once per frame
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            showing = 0f;
        }
    }

    void Update()
    {
        panel.alpha = Mathf.MoveTowards(panel.alpha, showing, fadeSpeed * Time.deltaTime);
    }
}
