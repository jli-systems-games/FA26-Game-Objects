using UnityEngine;

public class wiggleScript : MonoBehaviour
{
    public float distance = 10f;   // how far it moves left and right
    public float speed = 5f;       // how fast it goes back and forth

    Vector3 startPos;

    void Start()
    {
        startPos = transform.localPosition;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.unscaledTime * speed) * distance;
        transform.localPosition = startPos + new Vector3(offset, 0f, 0f);
    }
}