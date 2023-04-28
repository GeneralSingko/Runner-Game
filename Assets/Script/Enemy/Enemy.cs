using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public int damage = 10;  // The amount of damage the enemy does to the player
    public float attackDelay = 1f;  // The delay between attacks
    private bool canAttack = true;  // Whether or not the enemy can currently attack


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Disappear the enemy
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (canAttack)
            {
                collision.gameObject.GetComponent<PlayerHealth>().TakeDamage(damage);
                canAttack = false;
                Invoke("ResetAttack", attackDelay);
            }
        }

        if(collision.gameObject.tag == "Target")
        {
            if (canAttack)
            {
                collision.gameObject.GetComponent<Beacon>().TakeDamage(damage);
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
    }
}
