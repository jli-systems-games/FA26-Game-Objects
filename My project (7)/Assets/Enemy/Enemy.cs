using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    // 移动边界（上下范围，根据你的背景图片调整）
    public float topBound;
    public float bottomBound;

    HurtSound hurtSound;

    void Start()
    {
        hurtSound = FindObjectOfType<HurtSound>();
    }

    void Update()
    {
        // 上下移动，只改变Y轴
        transform.Translate(0, moveSpeed * Time.deltaTime, 0);

        Debug.Log(Time.deltaTime);

        // 碰到上下边界，速度取反，掉头
        if (transform.position.y >= topBound)
        {
            moveSpeed = -Mathf.Abs(moveSpeed);
        }
        else if (transform.position.y <= bottomBound)
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