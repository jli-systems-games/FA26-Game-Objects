using UnityEngine;

public class SpearAttack : MonoBehaviour
{
    public Transform enemy;       
    public Transform player;      
    public GameObject spearModel;  

    public float attackRange = 3f;      //攻击判定举例  attackRange
    public float aimSpeed = 180f;       //瞄准速度  aimSpeed
    public float thrustDistance = 1.5f; // 攻击距离 thrustDistance
    public float attackTime = 0.2f;    // 前摇 attackTime
    public float endTime = 0.35f;    // 后摇  endTime

    private bool isAttacking = false;  //防重复执行 isAttacking
    private float timer = 0f;         //执行到第几秒 timer
    private bool playerInAttackRange = false;


    private Vector3 startPosition;  // 长枪复原位置 startPosition

    void OnEnable()
    {
        startPosition = spearModel.transform.localPosition;

        playerInAttackRange = false;
        isAttacking = false;
        timer = 0f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))playerInAttackRange = true;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))playerInAttackRange = false;
    }

    void Update()
    {
        if (isAttacking)
        {
            SpearAttackAction();
            Debug.Log("attack");
            return;  
        }

 


        Vector3 targetDirection = player.position - enemy.position;
        //不抬头
        targetDirection.y = 0f;



          //不在瞄准范围
        if (targetDirection.magnitude > attackRange) return; 


            //在攻击范围且在瞄准范围
        if (playerInAttackRange)
        {
            isAttacking = true;   //下一帧执行攻击
            timer = 0f;
        }
            //在攻击范围但不在瞄准范围
        else if (!playerInAttackRange)
        {
  
            Quaternion facing = Quaternion.LookRotation(targetDirection);
            enemy.rotation = Quaternion.RotateTowards( enemy.rotation, facing, aimSpeed * Time.deltaTime);
        }
    }

    void SpearAttackAction()
    {
        timer += Time.deltaTime;

        Vector3 endPosition = startPosition;
        endPosition.z += thrustDistance;

        if (timer < attackTime)
        {
            spearModel.transform.localPosition = Vector3.Lerp(startPosition, endPosition, timer / attackTime);
        }
        else if (timer < attackTime + endTime)
        {
            float returnTime = timer - attackTime;
            spearModel.transform.localPosition = Vector3.Lerp(endPosition, startPosition, returnTime / endTime);
        }
        else
        {
            spearModel.transform.localPosition = startPosition;
            isAttacking = false;
        }
    }
}
