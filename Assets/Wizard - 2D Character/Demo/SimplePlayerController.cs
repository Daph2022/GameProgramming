using UnityEngine;

namespace ClearSky
{
    public class SimplePlayerController : MonoBehaviour
    {
        public float movePower = 10f;
        public float jumpPower = 15f; // Gravity Scale dans Rigidbody2D à 5

        private Rigidbody2D rb;
        private Animator anim;
        private int direction = 1;
        private bool isGrounded = false;
        private bool alive = true;
        public float attackRange = 2f;
        public LayerMask enemyLayer;
        public GameObject fireballPrefab;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            Restart();
            if (alive)
            {
                Hurt();
                Die();
                Attack();
                Jump();
                Run();
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
                anim.SetBool("isJump", false);
            }
        }

        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
                anim.SetBool("isJump", true);
            }
        }

        void Run()
        {
            Vector3 moveVelocity = Vector3.zero;
            anim.SetBool("isRun", false);

            if (Input.GetAxisRaw("Horizontal") < 0)
            {
                direction = -1;
                moveVelocity = Vector3.left;
                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);
            }
            else if (Input.GetAxisRaw("Horizontal") > 0)
            {
                direction = 1;
                moveVelocity = Vector3.right;
                transform.localScale = new Vector3(direction, 1, 1);
                if (!anim.GetBool("isJump"))
                    anim.SetBool("isRun", true);
            }

            transform.position += moveVelocity * movePower * Time.deltaTime;
        }

        void Jump()
        {
            if ((Input.GetButtonDown("Jump") || Input.GetAxisRaw("Vertical") > 0) && isGrounded)
            {
                anim.SetBool("isJump", true);
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f); // Reset vitesse verticale
                rb.AddForce(new Vector2(0f, jumpPower), ForceMode2D.Impulse);
                isGrounded = false;
            }
        }

        void Attack()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                anim.SetTrigger("attack");
                Vector3 fireballPosition = transform.position + new Vector3(0.5f * direction, 2.5f, 0);
                GameObject fireball = Instantiate(fireballPrefab, fireballPosition, Quaternion.identity);
                fireball.transform.localScale = new Vector3(direction * 0.19f, 0.19f, 0.19f);
            }
        }

        void Hurt()
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                anim.SetTrigger("hurt");
                if (direction == 1)
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector2(5f, 1f), ForceMode2D.Impulse);
            }
        }

        void Die()
        {
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                anim.SetTrigger("die");
                alive = false;
            }
        }

        void Restart()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                anim.SetTrigger("idle");
                alive = true;
            }
        }
    }
}
