using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Monkey parent;
    public float lifeTime = 5f;

    private int pierce;
    
    private void Start()
    {
        Destroy(gameObject, lifeTime);
        pierce = parent.pierce;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        Bloon b = col.GetComponent<Bloon>();
        if (b != null)
        {
            if (pierce <= 0)
            {
                Destroy(gameObject);
                return;
            }
            pierce--;
            b.TakeDamage(parent.damage, parent.DamageTypes);
            if (parent.isExplosive)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2f);
                foreach (Collider2D c in colliders)
                {
                    Bloon bloon = c.gameObject.GetComponent<Bloon>();
                    if (bloon != null)
                        bloon.TakeDamage(parent.damage, parent.DamageTypes);
                }
            }
            
        }
        if (pierce <= 0)
        {
            Destroy(gameObject);
            return;
        }
    }


    private void Update()
    {
        transform.position += transform.up * parent.speed * Time.deltaTime;
    }
}


public enum damageType
{
    Standard,
    Explosive,
    Fire,
    Ice
}
