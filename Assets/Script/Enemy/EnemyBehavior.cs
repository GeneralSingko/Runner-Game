using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public Transform target;
    public float moveSpeed = 5f;
    public float stoppingDistance = 1f;
    public float detectionDistance = 5f;
    public int damageAmount = 10;

    private bool isChasing = false;
    private bool hasCollidedWithTarget = false;
    private Vector2 originalTarget;

    private Rigidbody2D rb;
    private Collider2D col;
    private Transform player;



    private void Start()
    {
        originalTarget = target.position;
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void FixedUpdate()
    {
        if (!isChasing)
        {
            MoveTowardsTarget();
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    private void MoveTowardsTarget()
    {
        // Move towards the original target
        Vector2 direction = (originalTarget - (Vector2)transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Check if the enemy has reached the target
        if (Vector2.Distance(transform.position, originalTarget) < stoppingDistance)
        {
            // Destroy the enemy
            Destroy(gameObject);
        }
        else if (Vector2.Distance(transform.position, player.position) < detectionDistance)
        {
            // Switch to chasing mode if player is within detection distance
            isChasing = true;
        }
    }

    private void MoveTowardsPlayer()
    {
        // Move towards the player
        Vector2 direction = (player.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;

        // Check if the enemy has reached the player
        if (Vector2.Distance(transform.position, player.position) < stoppingDistance)
        {
            // Stop chasing the player and move towards the original target
            isChasing = false;
        }
        else if (Vector2.Distance(transform.position, player.position) > detectionDistance)
        {
            // Player is out of range, switch back to moving towards original target
            isChasing = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the enemy collided with the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Stop moving
            rb.velocity = Vector2.zero;

            // Get the direction from the player to the enemy
            Vector2 direction = (transform.position - player.position).normalized;

            // Push the enemy away from the player
            rb.AddForce(direction * moveSpeed * 10f, ForceMode2D.Impulse);

            // Move towards the original target after being pushed
            StartCoroutine(MoveTowardsTargetAfterDelay());
        }
        // Check if the enemy collided with the target
        else if (collision.gameObject.CompareTag("Target"))
        {
            if (!hasCollidedWithTarget)
            {
                hasCollidedWithTarget = true;

                // apply damage to the target
                Beacon target = collision.gameObject.GetComponent<Beacon>();
                target.TakeDamage(damageAmount);

                // destroy the enemy
                Destroy(gameObject);
            }

        }
    }

    private IEnumerator MoveTowardsTargetAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        isChasing = false;
    }
}
