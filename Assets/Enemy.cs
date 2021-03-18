using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    [SerializeField]private int health;
    private Rigidbody2D rb;
    private Animator animator;
    public AIPath aiPath;
    // Start is called before the first frame update

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        aiPath.enabled = false;
        transform.localScale = new Vector3(1f, 1f, 1f);
        animator.SetTrigger("Death");
    }

    // Update is called once per frame
    void Update()
    {
        if( Mathf.Abs( rb.velocity.x) > 0f)
        {
            animator.SetBool("Walking", true);
        }

        if(aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if(aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }
    void Destroy()
    {
        Destroy(this.gameObject);
    }
}
