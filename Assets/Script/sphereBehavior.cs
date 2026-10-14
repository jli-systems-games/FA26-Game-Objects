using UnityEngine;

public class sphereBehavior : MonoBehaviour
{
    [SerializeField]
    private float fallSpeed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fallSpeed = Random.Range(2, 9);
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(gameObject, Random.Range(2, 6));
        gameObject.transform.position += new Vector3(0, -fallSpeed * Time.deltaTime, 0);
    }
}
