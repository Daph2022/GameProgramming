using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject pointA;
    public GameObject pointB;
    private Rigidbody2D rb;
    private Transform currentPoint; public int health = 2;
    private Animator anim;
    private bool isDead = false;
    public float speed = 2f;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentPoint = pointB.transform;
        anim.SetBool("isWalking", true);
    }

    public void TakeDamage()
    {
        if (isDead) return;

        health--;
        anim.SetTrigger("hit");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        anim.SetBool("isDead", true);

        // Destruction avec délai pour voir animation
        Destroy(gameObject, 2f);
    }

    void Update()
    {
        Vector2 point = currentPoint.position - transform.position;
        if (currentPoint == pointB.transform)
        {
            rb.linearVelocity = new Vector2(speed, 0);
        }
        else
        {
            rb.linearVelocity = new Vector2(-speed, 0);
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointB.transform)
        {
            flip();
            currentPoint = pointA.transform;
        }

        if (Vector2.Distance(transform.position, currentPoint.position) < 0.5f && currentPoint == pointA.transform)
        {
            flip();
            currentPoint = pointB.transform;
        }
        //bool walking = false; // à remplacer par ta logique de déplacement
        //anim.SetBool("isWalking", walking);
    }

    private void flip()
    {
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(pointA.transform.position, 0.5f);
        Gizmos.DrawWireSphere(pointB.transform.position, 0.5f);
        Gizmos.DrawLine(pointA.transform.position, pointB.transform.position);
    }
}
