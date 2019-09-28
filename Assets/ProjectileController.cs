using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private int spellDamage;
    private Rigidbody2D rigidBody;
    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Shoot(int xmove, int ymove, int damage, int speed)
    {
        spellDamage = damage;
        print(damage);
        print(xmove);
        print(ymove);
        rigidBody.transform.Translate(1,1,1);
    }
}