
using UnityEngine;

public class FishMovement : MonoBehaviour
{
    public float speed = 100f;
    public float distance = 600f;
    public bool moveRight = true;

    private RectTransform rect;
    private Vector2 startPosition;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        startPosition = rect.anchoredPosition;
    }

    void Update()
    {
        float movement = Mathf.Repeat(Time.time * speed, distance);

        if (moveRight)
        {
            rect.anchoredPosition = startPosition + Vector2.right * movement;
        }
        else
        {
            rect.anchoredPosition = startPosition + Vector2.left * movement;
        }
    }
}
