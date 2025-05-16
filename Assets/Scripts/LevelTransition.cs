using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelTransition : MonoBehaviour
{
    public static LevelTransition instance;

    public CanvasGroup transitionCanvas;
    public AudioSource transitionSound;
    public float fadeDuration = 1f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FadeToScene(string sceneName)
    {
        StartCoroutine(FadeAndLoad(sceneName));
    }

    IEnumerator FadeAndLoad(string sceneName)
    {
        // Jouer le son de transition
        if (transitionSound != null)
            transitionSound.Play();

        // Faire un fondu noir
        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            transitionCanvas.alpha = timer / fadeDuration;
            yield return null;
        }

        // Charger la nouvelle scène
        SceneController.instance.LoadScene(sceneName);
    }
}
