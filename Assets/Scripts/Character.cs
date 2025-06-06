using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public float MaxHP = 3f;
    float HP;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }


    //살아있으면  true를 리턴한다.
    public bool Hit(float damage)
    {
        HP -= damage;

        if(HP < 0)
        {
            HP = 0;
        }
        return HP > 0;
    }
}
