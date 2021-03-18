using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]private float speed = 20;
    [SerializeField]private int damage = 40;
    private Rigidbody2D rb;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Destroy(this.gameObject, 10);
    }
    void Start()
    {
        rb.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if(enemy != null)
        {
            rb.velocity = new Vector2(0f, 0f);
            enemy.TakeDamage(damage);
            animator.SetTrigger("Impact");
        }
        Debug.Log(collision.name);
    }

    private void Delete()
    {
        Destroy(this.gameObject);
    }
}
