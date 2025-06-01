using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCountroller : MonoBehaviour
{
    public float speed = 1f;

    Vector3 move;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        move = Vector3.zero;

        if (Input.GetKey(KeyCode.A))
            move += new Vector3(-1, 0, 0);
        if (Input.GetKey(KeyCode.D))
            move += new Vector3(1, 0, 0);
        if (Input.GetKey(KeyCode.W))
            move += new Vector3(0, 1, 0);
        if (Input.GetKey(KeyCode.S))
            move += new Vector3(0, -1, 0);

        move = move.normalized;

        if (move.x < 0)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (move.x > 0)
            GetComponent<SpriteRenderer>().flipX = false;

        //애니메이션
        if(move.magnitude > 0)
        {
            GetComponent<Animator>().SetTrigger("Move");
        }
        else
        {
            GetComponent<Animator>().SetTrigger("Stop");
        }
    }

    private void FixedUpdate()
    {
        transform.Translate(move * speed * Time.fixedDeltaTime);
    }
}
