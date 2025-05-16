using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [Header("Paramètres de santé")]
    public int maxHealth = 100;
    public int currentHealth;
    
    [Header("Interface utilisateur")]
    public Slider healthSlider;
    public Image damageImage;
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip hurtSound;
    public AudioClip deathSound;
    
    private Animator animator;
    private bool isDead = false;
    private float flashSpeed = 5f;
    private Color flashColor = new Color(1f, 0f, 0f, 0.1f);
    private bool damaged = false;
    
    void Start()
    {
        // Initialiser la santé
        currentHealth = maxHealth;
        
        // Récupérer le composant Animator
        animator = GetComponent<Animator>();
        
        // Configurer le slider de santé
        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }
        
        // Rendre l'image de dégâts invisible
        if (damageImage != null)
        {
            Color color = damageImage.color;
            color.a = 0f;
            damageImage.color = color;
        }
    }
    
    void Update()
    {
        // Effet de flash quand le joueur prend des dégâts
        if (damaged && damageImage != null)
        {
            damageImage.color = flashColor;
        }
        else if (damageImage != null)
        {
            // Faire disparaître progressivement l'effet de dégâts
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }
        
        // Réinitialiser le flag de dégâts
        damaged = false;
    }
    
    public void TakeDamage(int damage)
    {
        // Ignorer les dégâts si déjà mort
        if (isDead) return;
        
        // Marquer comme ayant subi des dégâts pour l'effet visuel
        damaged = true;
        
        // Réduire la santé
        currentHealth -= damage;
        
        // Mettre à jour le slider de santé
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }
        
        // Jouer le son de dégâts
        if (audioSource != null && hurtSound != null)
        {
            audioSource.PlayOneShot(hurtSound);
        }
        
        // Jouer l'animation de dégâts si disponible
        if (animator != null)
        {
            animator.SetTrigger("Hurt");
        }
        
        // Vérifier si le joueur est mort
        if (currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }
    
    void Death()
    {
        isDead = true;
        
        // Jouer l'animation de mort si disponible
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        
        // Jouer le son de mort
        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }
        
        // Désactiver les contrôles du joueur
        // Cette partie désactive le script de mouvement du joueur
        // Remplacez "PlayerMovement" par le nom de votre script de contrôle du joueur
        // Par exemple: "PlayerController", "CharacterController2D", etc.
        MonoBehaviour playerController = GetComponent<MonoBehaviour>();
        if (playerController != null && playerController.GetType().Name.Contains("Player"))
        {
            playerController.enabled = false;
        }
        
        // Désactiver les collisions avec les ennemis
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }
        
        // Désactiver le Rigidbody2D pour arrêter le mouvement
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
}