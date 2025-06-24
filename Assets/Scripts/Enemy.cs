using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;
    public float attackRange = 3.0f; // ���� �Ÿ�
    public Rigidbody2D target;

    private bool isLive = true;

    private Rigidbody2D rb;
    private SpriteRenderer spriter;
    private Animator anim;

    private bool isAttacking = false;
    private float attackCooldown = 5.0f; // ���� ��ٿ� �ð� (��)
    private float lastAttackTime = 0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        if (!isLive || target == null)
            return;

        Vector2 dirVec = target.position - rb.position;
        float distance = dirVec.magnitude;

        // ���� �ִϸ����� ���� Ȯ�� (0�� �⺻ ���̾�)
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);

        // ���� �ִϸ��̼� ���̸� �̵� �� �߰� ���� Ʈ���� ����
        if (state.IsName("Attack"))
        {
            isAttacking = true;
            anim.SetBool("isWalking", false);
            return;
        }
        else
        {
            isAttacking = false;
        }

        if (distance <= attackRange)
        {
            anim.SetBool("isWalking", false);

            // ��ٿ� üũ �� ���� Ʈ����
            if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
            {
                anim.SetTrigger("Attack");
                isAttacking = true;
                lastAttackTime = Time.time;
            }
        }
        else
        {
            // ���ݹ��� ���̸� �̵�
            isAttacking = false;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + nextVec);

            anim.SetBool("isWalking", true);
        }
    }

    private void LateUpdate()
    {
        if (!isLive || target == null)
            return;

        spriter.flipX = target.position.x < rb.position.x;
    }

    // ���� �ִϸ��̼� ���� �� Animation Event�� ȣ��
    public void OnAttackAnimationEnd()
    {
        isAttacking = false;
    }
}
