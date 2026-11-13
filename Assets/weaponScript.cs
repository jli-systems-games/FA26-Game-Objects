using UnityEngine;

public class weaponScript : MonoBehaviour
{
float rotateSpeed = 90f; //degrees
       public float bobHeight = 0.2f; 
        public float bobSpeed = 2.3f;

        Vector3 startPos; //stores the starting position of obj

    void Start()
    {
        startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //y- axis
        transform.Rotate(0f, rotateSpeed * Time.deltaTime, 0f, Space.World);

        //bob
        float offset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPos + new Vector3(0f, offset, 0f);
    }
}
