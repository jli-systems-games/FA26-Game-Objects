using UnityEngine;

public class ChasingLogic : MonoBehaviour
{
    public Transform enemy;      
    public Transform player;      
    public int attackRange = 2;  
    public int sightRange = 10;   
    public float speed = 3f;      

    void Update()
    {
        float distance = Vector3.Distance(enemy.position, player.position);

        if (distance > attackRange && distance < sightRange)
        {
            enemy.position = Vector3.MoveTowards(
                enemy.position, player.position, speed * Time.deltaTime);
        }
    }
}
