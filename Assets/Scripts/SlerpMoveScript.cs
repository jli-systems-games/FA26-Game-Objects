using UnityEngine;

public class SlerpMoveScript : MonoBehaviour
{
    public GameObject objTarget;
    public float moveSpeed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, objTarget.transform.position, moveSpeed * Time.deltaTime);
    }
}
