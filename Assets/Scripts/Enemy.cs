using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    bool isLive;

    private Rigidbody2D rb;

    private GameObject target;
    private SpriteRenderer sprite;

    private void Start()
    {
        target = GameObject.FindWithTag("Player");
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (target == null) return;
        
        Vector2 dirVec = (target.transform.position - (Vector3)rb.position);
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;

        rb.MovePosition(rb.position + nextVec);

        sprite.flipX = dirVec.x < 0;
    }
}
