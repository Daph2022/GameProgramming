using UnityEngine;
using System.Collections;

public class DragonCombat : MonoBehaviour
{
    [Header("PV")]
    public int maxHealth = 5;
    private int currentHealth;

    [Header("Références")]
    public Animator animator;
    public Rigidbody2D rb;
    public Transform player;
    public AudioSource audioSource;

    [Header("Paramètres de combat")]
    public float moveSpeed = 2f;
    public float attackRange = 3f;
    public float specialAttackRange = 5f;
    public bool isFacingRight = true;

    [Header("Délais d'attaque")]
    public float attackCooldown = 2f;
    public float specialAttackCooldown = 5f;

    [Header("Dégâts")]
    public int attackDamage = 10;
    public int specialAttackDamage = 20;

    [Header("Audio")]
    public AudioClip attackSound;
    public AudioClip specialAttackSound;

    // États de combat
    private bool inCombat = false;
    private bool canAttack = true;
    private bool canSpecialAttack = true;
    private bool isAttacking = false;
    private bool isStuned = false;

    void Start()
    {
        currentHealth = maxHealth;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Bloquer toutes actions si étourdi ou en train d'attaquer ou si hors combat ou joueur manquant
        if (!inCombat || player == null || isAttacking || isStuned)
            return;

        // Mettre à jour l'orientation du dragon en fonction de la position du joueur
        UpdateFacing();

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            if (canAttack)
            {
                StartCoroutine(PerformAttack());
            }
            else
            {
                StopMovement();
            }
        }
        else if (distanceToPlayer <= specialAttackRange)
        {
            if (canSpecialAttack)
            {
                StartCoroutine(PerformSpecialAttack());
            }
            else if (canAttack)
            {
                StartCoroutine(PerformAttack());
            }
            else
            {
                MoveTowardsPlayer();
            }
        }
        else
        {
            MoveTowardsPlayer();
        }
    }

    // Nouvelle méthode pour mettre à jour l'orientation du dragon
    private void UpdateFacing()
    {
        if (player != null)
        {
            // Détermine si le dragon doit faire face à droite ou à gauche
            bool shouldFaceRight = player.position.x > transform.position.x;

            // Mettre à jour la variable isFacingRight seulement si nécessaire
            if (shouldFaceRight != isFacingRight)
            {
                isFacingRight = shouldFaceRight;
                
                // Si vous avez besoin de retourner le sprite
                // transform.localScale = new Vector3(isFacingRight ? 1 : -1, 1, 1);
            }
        }
    }

    public void StartCombat()
    {
        inCombat = true;
        animator.SetBool("InCombat", true);
        Debug.Log("Combat démarré !");
    }

    public void TakeDamage(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damage;
        Debug.Log($"Dragon prend {damage} dégâts, PV restants : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // Déclenche l'état étourdi
            StartCoroutine(Stun());
        }
    }

    private IEnumerator Stun()
    {
        isStuned = true;
        animator.SetTrigger("StunedTrigger");

        // Arrêter tout mouvement
        StopMovement();

        // Durée d'étourdissement = durée de l'animation + un peu de temps
        yield return new WaitForSeconds(2.0f);

        isStuned = false;
    }

    private void MoveTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;

        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        animator.SetBool("IsMoving", Mathf.Abs(rb.linearVelocity.x) > 0.1f);
    }

    private void StopMovement()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("IsMoving", false);
    }

    private IEnumerator PerformAttack()
    {
        isAttacking = true;
        canAttack = false;

        StopMovement();

        animator.SetTrigger("AttackTrigger");

        if (audioSource != null && attackSound != null)
            audioSource.PlayOneShot(attackSound);

        yield return new WaitForSeconds(0.5f);

        CheckHitPlayer(attackRange, attackDamage);

        yield return new WaitForSeconds(0.5f);

        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator PerformSpecialAttack()
    {
        isAttacking = true;
        canSpecialAttack = false;

        StopMovement();

        animator.SetTrigger("SpecialATrigger");

        if (audioSource != null && specialAttackSound != null)
            audioSource.PlayOneShot(specialAttackSound);

        yield return new WaitForSeconds(0.8f);

        CheckHitPlayer(specialAttackRange, specialAttackDamage);

        yield return new WaitForSeconds(0.7f);

        isAttacking = false;

        yield return new WaitForSeconds(specialAttackCooldown);
        canSpecialAttack = true;
    }

    private void CheckHitPlayer(float range, int damage)
    {
        if (player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= range)
        {
            // Utiliser GetComponent directement sur le joueur au lieu du Raycast
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Dragon a touché le joueur et infligé {damage} points de dégâts !");
            }
            else
            {
                Debug.LogWarning("Le joueur n'a pas de composant PlayerHealth !");
            }
        }
    }

    private void Die()
    {
        Debug.Log("Dragon est mort !");
        animator.SetTrigger("DeathTrigger");

        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
        GetComponent<Collider2D>().enabled = false;

        Destroy(gameObject, 3f);
    }

    // Pour le débogage visuel
    void OnDrawGizmosSelected()
    {
        // Visualiser la portée d'attaque
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        
        // Visualiser la portée d'attaque spéciale
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, specialAttackRange);
    }
}