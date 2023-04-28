using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    private bool isAlive = true;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (!isAlive)
        {
            return;
        }

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            isAlive = false;
            Die();
        }
    }

    void Die()
    {
        // Stop player movement
        GetComponent<PlayerController>().enabled = false;
    }
}
