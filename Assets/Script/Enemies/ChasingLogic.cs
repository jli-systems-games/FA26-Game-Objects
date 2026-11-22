using UnityEngine;

public class ChasingLogic : MonoBehaviour
{
    public Transform enemy;      
    public Transform player;      
    public int attackRange = 2;  
    public int sightRange = 10;   
    public float speed = 1f; 
    public float turnSpeed = 360f;     

    void Update()
    {
        if (Vector3.Distance(player.position, enemy.position) > sightRange ||
         Vector3.Distance(player.position, enemy.position) < attackRange)
        {
            return;
        }

        Quaternion facing = Quaternion.LookRotation(player.position - enemy.position);

        enemy.rotation = Quaternion.RotateTowards(enemy.rotation, facing, turnSpeed * Time.deltaTime);

        float distance = Vector3.Distance(enemy.position, player.position);

        if (Quaternion.Angle(enemy.rotation, facing) < 5f)
        {
            enemy.position = Vector3.MoveTowards( enemy.position, player.position, speed * Time.deltaTime);
        }
    }
}
