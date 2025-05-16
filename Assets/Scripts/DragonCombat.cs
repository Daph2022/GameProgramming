using UnityEngine;
using System.Collections;

public class DragonCombat : MonoBehaviour
{
    [Header("PV")]
    public int maxHealth = 10;
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

    void Start()
    {
        currentHealth = maxHealth;
        // Si le joueur n'est pas assigné, le trouver par tag
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
        }

        // S'assurer que les composants nécessaires sont assignés
        if (animator == null)
            animator = GetComponent<Animator>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!inCombat) return;

        if (player != null && !isAttacking)
        {
            // Calculer la distance au joueur
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // Décider de l'action en fonction de la distance
            if (distanceToPlayer <= attackRange)
            {
                // À portée d'attaque normale
                if (canAttack)
                {
                    StartCoroutine(PerformAttack());
                }
            }
            else if (distanceToPlayer <= specialAttackRange)
            {
                // À portée d'attaque spéciale
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
                    // Se déplacer vers le joueur
                    MoveTowardsPlayer();
                }
            }
            else
            {
                // Trop loin, se déplacer vers le joueur
                MoveTowardsPlayer();
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
        if (currentHealth <= 0) return; // déjà mort, on ignore

        currentHealth -= damage;
        Debug.Log($"Dragon prend {damage} dégâts, PV restants : {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("StunedTrigger");
        }
    }

    private void MoveTowardsPlayer()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;

        // Déplacement du dragon
        rb.linearVelocity = new Vector2(direction.x * moveSpeed, rb.linearVelocity.y);

        // Animation
        animator.SetBool("IsMoving", Mathf.Abs(rb.linearVelocity.x) > 0.1f);

        // Flip du sprite
        if (direction.x > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (direction.x < 0 && isFacingRight)
        {
            Flip();
        }
    }


    private void Flip()
    {
        // Inverser la direction du sprite
        isFacingRight = !isFacingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private IEnumerator PerformAttack()
    {
        // Préparer l'attaque
        isAttacking = true;
        canAttack = false;

        // Arrêter le mouvement
        rb.linearVelocity = Vector2.zero;

        // Déclencher l'animation d'attaque
        animator.SetTrigger("AttackTrigger");

        // Jouer le son d'attaque
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Attendre que l'animation atteigne le point d'impact
        yield return new WaitForSeconds(0.5f); // Ajuster selon votre animation

        // Vérifier si le joueur est à portée pour infliger des dégâts
        CheckHitPlayer(attackRange, attackDamage);

        // Attendre que l'animation d'attaque se termine
        yield return new WaitForSeconds(0.5f); // Ajuster selon votre animation

        // Réinitialiser les états
        isAttacking = false;

        // Appliquer le cooldown
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator PerformSpecialAttack()
    {
        // Préparer l'attaque spéciale
        isAttacking = true;
        canSpecialAttack = false;

        // Arrêter le mouvement
        rb.linearVelocity = Vector2.zero;

        // Déclencher l'animation d'attaque spéciale
        animator.SetTrigger("SpecialATrigger");

        // Jouer le son d'attaque spéciale
        if (audioSource != null && specialAttackSound != null)
        {
            audioSource.PlayOneShot(specialAttackSound);
        }

        // Attendre que l'animation atteigne le point d'impact
        yield return new WaitForSeconds(0.8f); // Ajuster selon votre animation

        // Vérifier si le joueur est à portée pour infliger des dégâts
        CheckHitPlayer(specialAttackRange, specialAttackDamage);

        // Attendre que l'animation d'attaque spéciale se termine
        yield return new WaitForSeconds(0.7f); // Ajuster selon votre animation

        // Réinitialiser les états
        isAttacking = false;

        // Appliquer le cooldown
        yield return new WaitForSeconds(specialAttackCooldown);
        canSpecialAttack = true;
    }

    private void CheckHitPlayer(float range, int damage)
    {
        // Vérifier si le joueur est à portée
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer <= range)
            {
                // Déterminer la direction de l'attaque
                Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

                // Effectuer un Raycast pour vérifier s'il y a des obstacles
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, range);

                if (hit.collider != null && hit.collider.CompareTag("Player"))
                {
                    // Infliger des dégâts au joueur
                    PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damage);
                    }
                }
            }
        }
    }

    private void Die()
    {
        Debug.Log("Dragon est mort !");
        animator.SetTrigger("DeathTrigger");
        // Bloquer les actions du dragon, désactiver le script par exemple
        this.enabled = false;
        rb.linearVelocity = Vector2.zero;
        // Tu peux aussi désactiver le collider pour qu'il ne prenne plus de dégâts
        GetComponent<Collider2D>().enabled = false;
        // Ajouter une destruction différée si tu veux
        Destroy(gameObject, 3f);
    }


}