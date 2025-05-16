using UnityEngine;
using TMPro;

public class Interact : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private Animator animator;
    public TextMeshProUGUI interactionText;

    void Start()
    {
        animator = GetComponent<Animator>();
        interactionText.gameObject.SetActive(false); // cache le texte au début
    }

    void Update()
    {
        if (isPlayerNearby && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Ouvrir la boîte !");
            animator.SetBool("isBoxOpen", true);
            GetComponent<Collider2D>().enabled = false;
            interactionText.gameObject.SetActive(false); // cache le texte après ouverture
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = true;
            interactionText.gameObject.SetActive(true); // affiche le texte
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactionText.gameObject.SetActive(false); // cache le texte
        }
    }
}
