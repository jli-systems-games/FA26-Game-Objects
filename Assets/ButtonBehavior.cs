using UnityEngine;
using UnityEngine.UI;


public class ButtonBehavior : MonoBehaviour
{
    public Button button;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        button.onClick.AddListener(DoOneThing);
    }

    // Update is called once per frame

    private void OnDestroy()
    {
        button.onClick.RemoveListener(DoOneThing);
    }
    public void DoOneThing()
    {
        Debug.Log("Did one thing");
    }

    public void DoAnotherThing()
    {
        Debug.Log("Did another thing");
    }

}
