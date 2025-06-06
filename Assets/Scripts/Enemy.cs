using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3f;

    GameObject target;

    private void Start()
    {
        target = GameObject.Find("Player");
    }

    private void FixedUpdate()
    {
        Vector2 direction = target.transform.position - transform.position;

        transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);

        if(direction.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        if (direction.x > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
