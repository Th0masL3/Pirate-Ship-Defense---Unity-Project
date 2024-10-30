using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monkey : MonoBehaviour
{
    public string name;

    public float fireRate = 1;
    public float radius = 1;

    public int damage = 1;
    public float speed = 11;
    public int pierce = 1;
    public bool isExplosive = false;
    public damageType[] DamageTypes;

    public int cost = 30;
    public GameObject radiusDisplay;
    public Projectile proj;
    public GameObject mesh;

    private float nextFire;
    private void Update()
    {
        if (Time.time > nextFire)
        {
            Bloon b = getClosesetToEnd();
            if (b != null)
            {
                nextFire = Time.time + fireRate;
                fire(b);
            }

        }
    }


    public void selected(bool v)
    {
        radiusDisplay.SetActive(v);
        radiusDisplay.transform.localScale = new Vector2(radius * 2, radius * 2);
    }

    private Bloon[] getAllBloonsInRange()
    {
        Collider2D[] col = Physics2D.OverlapCircleAll(transform.position, radius);
        Bloon[] bloons = new Bloon[col.Length];
        if (col.Length == 0) return bloons;
        int i = 0;
        foreach (var c in col)
        {
            Bloon b = c.gameObject.GetComponent<Bloon>();
            if (b != null)
            {
                bloons[i] = b;
            }

            i++;
        }

        return bloons;
    }


    private Bloon getClosesetToEnd()
    {
        Bloon[] bloons = getAllBloonsInRange();
        Bloon closest = null;
        float distance = float.MaxValue;
        foreach (Bloon b in bloons)
        {
            if(b == null) continue;

            float d = b.GetDistanceLeft();
            if (d < distance)
            {
                distance = d;
                closest = b;
            }
        }

        return closest;
    }

    private void fire(Bloon b)
    {
        float dist = Vector2.Distance(transform.position, b.transform.position);
        Vector2 prediction = (Vector2)b.transform.position - ((b.velocity * dist / speed) * 2);
        transform.up = prediction - (Vector2)transform.position;
        Projectile p = Instantiate(proj, transform.position, transform.rotation);
        p.parent = this;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
