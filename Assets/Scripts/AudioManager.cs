using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource SFXSource;

    public AudioClip background;

    private static AudioManager instance;

    private void Awake()
    {
        // S'il n'y a pas encore d'instance, on la crée et on garde cet objet entre les scènes
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Sinon on détruit le doublon
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.clip = background;
            musicSource.Play();
        }
    }
}
