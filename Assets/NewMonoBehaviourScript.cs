using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public string MyString = "banana";
    public float MyFloat = 4.35f;
    public GameObject MyGameObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        MyGameObject = GameObject.Find("Test Sphere");
    }

    // Update is called once per frame // this is a comment or note for me
    void Update()
    {
        //Automatically a comment, forward slashes can also be used as a placeholder to not activate incomplete code
        

        //MyFloat 
    }
}
