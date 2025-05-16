using UnityEngine;
using System.Collections;

public class DragonTrigger : MonoBehaviour
{
    public Animator dragonAnimator;
    public Rigidbody2D dragonRb;
    public float jumpForce = 10f;
    public bool hasJumped = false;
    public bool combatStarted = false;
    
    // Référence au script de combat du dragon
    public DragonCombat dragonCombat;
    
    // Audio source pour les sons
    public AudioSource audioSource;
    public AudioClip detectionSound;
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasJumped && other.CompareTag("Player"))
        {
            Debug.Log("Joueur détecté !");
            StartCoroutine(StartDragonSequence());
        }
    }
    
    private IEnumerator StartDragonSequence()
    {
        // Jouer le son de détection si disponible
        if (audioSource != null && detectionSound != null)
        {
            audioSource.PlayOneShot(detectionSound);
        }
        
        // Déclencher l'animation de saut ou d'alerte
        dragonAnimator.SetTrigger("JumpTrigger");
        
        // Changer le type de rigidbody pour permettre le mouvement
        dragonRb.bodyType = RigidbodyType2D.Dynamic;
        
        // Appliquer la force de saut
        dragonRb.linearVelocity = new Vector2(dragonRb.linearVelocity.x, jumpForce);
        hasJumped = true;
        
        // Attendre que l'animation de saut/introduction se termine
        // Ajustez ce délai en fonction de la durée de votre animation
        yield return new WaitForSeconds(2.5f);
        
        // Démarrer le combat
        if (dragonCombat != null)
        {
            combatStarted = true;
            dragonCombat.StartCombat();
        }
        
        // Désactiver ce collider pour qu'il ne se déclenche plus
        GetComponent<Collider2D>().enabled = false;
    }
}