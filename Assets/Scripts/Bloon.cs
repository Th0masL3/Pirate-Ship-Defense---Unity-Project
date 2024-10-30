using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bloon : MonoBehaviour
{
    public float speed = 1.5f;
    public int damage = 1;
    public int health = 1;
    public int worth = 1;
    public Bloon[] children;
    public Vector2 velocity;
    public damageType[] resistances;

    private int index;


    private void Update()
    {
        velocity = ((Vector2)transform.position - GameManager.instance.track.getWaypoint(index)).normalized * speed;
        transform.position = getNextPosition();
        if (Vector2.Distance(transform.position, GameManager.instance.track.getWaypoint(index)) < 0.2f)
        {
            index++;
            if (index >= GameManager.instance.track.waypoints.Length)
            {
                GameManager.instance.lives -= damage;
                Destroy(gameObject);

                if (GameManager.instance.lives <= 0)
                {
                    GameOver();
                }
            }
        }
    }

    private void die()
    {
        for (int i = 0; i < children.Length; i++)
        {
            GameObject b = Instantiate(children[i].gameObject, transform.position, transform.rotation);
            b.GetComponent<Bloon>().index = this.index;
        }

        GameManager.instance.money += worth;
        GameManager.instance.enemiesDefeated++;
        Destroy(gameObject);
    }

    public void TakeDamage(int damage, damageType[] damageTypes)
    {
        health -= damage;
        if (health <= 0)
        {
            die();
        }
    }


    public float GetDistanceLeft()
    {
        return GameManager.instance.track.getCumulitveDistance(index, transform.position);
    }

    private Vector2 getNextPosition()
    {
        Vector2 nextPosition = GameManager.instance.track.getWaypoint(index);
        Vector2 direction = nextPosition - (Vector2)transform.position;
        return (Vector2)transform.position + direction.normalized * speed * Time.deltaTime;
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

}