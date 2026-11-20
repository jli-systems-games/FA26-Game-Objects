using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemylr : MonoBehaviour
{
    public float moveSpeed;
    // 左右边界（X轴）
    public float leftBound;
    public float rightBound;

    HurtSound hurtSound;

    void Start()
    {
        hurtSound = FindObjectOfType<HurtSound>();
    }

    void Update()
    {
        // ========== 只改这里！左右移动，修改X轴 ==========
        transform.Translate(moveSpeed * Time.deltaTime, 0, 0);

        // 碰到左右边界，掉头反向
        if (transform.position.x >= rightBound)
        {
            moveSpeed = -Mathf.Abs(moveSpeed);
        }
        else if (transform.position.x <= leftBound)
        {
            moveSpeed = Mathf.Abs(moveSpeed);
        }
    }


    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = col.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
}