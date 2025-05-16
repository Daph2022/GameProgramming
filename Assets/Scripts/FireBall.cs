using UnityEngine;

public class Fireball : MonoBehaviour
{
    public float speed = 10f; // Vitesse de la boule de feu
    public int damage = 1;    // Dégâts infligés
    public float maxDistance = 10f; // Distance maximale avant que la boule disparaisse
    public GameObject fireballEffect; // Prefab de l'effet de particules de la boule de feu
    public GameObject impactEffect;   // Prefab de l'effet d'impact (explosion ou autre)
    public float activationDelay = 1f; 
    private Rigidbody2D rb;
    private Vector3 startPosition;
    private GameObject fireballEffectInstance; // Instance de l'effet de la boule de feu

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position; // Enregistre la position de départ de la boule de feu
        rb.linearVelocity = transform.right * speed; // Lance la boule de feu dans la direction du joueur

        // Instancie l'effet de la boule de feu à la position de départ
        if (fireballEffect != null)
        {
            fireballEffectInstance = Instantiate(fireballEffect, transform.position, transform.rotation, transform); // Attache l'effet à la boule de feu
        }
    }

    void Update()
    {
        // Vérifie si la boule de feu a atteint la distance maximale
        if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        {
            Destroy(gameObject); // Détruire la boule de feu après la distance maximale
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))  // Si la boule touche un ennemi
        {
            // Instancier l'effet d'impact
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
            }

            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage();
            }

            DragonCombat dragon = other.GetComponent<DragonCombat>();
            if (dragon != null)
            {
                dragon.TakeDamage(damage);
            }
            Destroy(gameObject);  // Détruire la boule de feu
        }
        else if (other.CompareTag("Ground") || other.CompareTag("Wall"))  // Si la boule touche le sol ou un mur
        {
            // Instancier l'effet d'impact
            if (impactEffect != null)
            {
                Instantiate(impactEffect, transform.position, transform.rotation);
            }

            Destroy(gameObject);  // Détruire la boule de feu
        }
    }

    private void OnDestroy()
    {
        // Détruire l'effet de la boule de feu quand la boule de feu est détruite
        if (fireballEffectInstance != null)
        {
            Destroy(fireballEffectInstance);
        }
    }
}
