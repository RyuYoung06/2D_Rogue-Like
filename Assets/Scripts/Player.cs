using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float speed = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    public float hp = 20f; // 플레이어 체력
    public float attackPower = 5f; // 플레이어 공격력
    public GameObject damageTextPrefab; // 인스펙터에서 프리팹 할당
    public Transform damageTextSpawnPoint; // (선택) 텍스트가 뜰 위치

    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    private Vector2 move; // ���� ������ �̵� ����
    private bool isDashing = false;
    private float lastDashTime = -Mathf.Infinity;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isDashing)
        {
            float moveX = Input.GetAxisRaw("Horizontal");
            float moveY = Input.GetAxisRaw("Vertical");
            move = new Vector2(moveX, moveY).normalized;

            // �ִϸ��̼� ���� �� �̵� ���� ����
            if (move != Vector2.zero)
            {
                anim.SetBool("isMoving", true);
                anim.SetFloat("MoveX", move.x);
                anim.SetFloat("MoveY", move.y);
                anim.SetFloat("LastMoveX", move.x);
                anim.SetFloat("LastMoveY", move.y);
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }

        // ��� �Է�
        if (Input.GetKeyDown(KeyCode.Space) && Time.time >= lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (!isDashing)
        {
            Vector2 nextVec = move * speed * Time.fixedDeltaTime;
            rb.MovePosition(rb.position + nextVec);
        }
    }

    IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        // move�� 0�̸� ������ ���� ���
        Vector2 dashDirection = move != Vector2.zero ? move : new Vector2(anim.GetFloat("LastMoveX"), anim.GetFloat("LastMoveY"));
        rb.velocity = dashDirection * dashSpeed;

        anim.SetFloat("MoveX", dashDirection.x);
        anim.SetFloat("MoveY", dashDirection.y);
        anim.SetBool("isDashing", true);
        anim.SetBool("isMoving", true);

        yield return new WaitForSeconds(dashDuration);

        rb.velocity = Vector2.zero;
        isDashing = false;
        move = Vector2.zero;

        anim.SetBool("isDashing", false);
        anim.SetBool("isMoving", false);
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        ShowDamageText(damage);
        Debug.Log($"플레이어 데미지: {damage}, 남은 HP: {hp}");
        if (hp <= 0)
        {
            Die();
        }
    }

    void ShowDamageText(int damage)
    {
        if (damageTextPrefab != null)
        {
            // 월드 공간 Canvas라면 Instantiate 위치를 transform.position로!
            GameObject obj = Instantiate(damageTextPrefab, damageTextSpawnPoint != null ? damageTextSpawnPoint.position : transform.position, Quaternion.identity);
            DamageText dmgText = obj.GetComponent<DamageText>();
            if (dmgText != null)
                dmgText.SetText(damage);
        }
    }

    private void Die()
    {
        Debug.Log("플레이어가 사망했습니다.");
        Debug.Log("죽었습니다");
    }
    
    void ShowDamageText(float damage)
    {
        if (damageTextPrefab != null)
        {
            Vector3 spawnPos = damageTextSpawnPoint != null ? damageTextSpawnPoint.position : transform.position;
            GameObject obj = Instantiate(damageTextPrefab, spawnPos, Quaternion.identity, damageTextPrefab.transform.parent);
            DamageText dmgText = obj.GetComponent<DamageText>();
            if (dmgText != null)
                dmgText.SetText(damage);
        }
    }
}
