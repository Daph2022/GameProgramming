using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour
{
    public GameObject player;
    public Transform respawnPoint;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

  private void OnCollisionEnter2D(Collision2D other) 
  {
    if(other.gameObject.CompareTag("Player"))
    {
        player.transform.position = respawnPoint.position;
        //Scene currentScene = SceneManager.GetActiveScene();;
        //SceneManager.LoadScene(currentScene.name);
    }
  }

}
