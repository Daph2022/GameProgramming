using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CreditsPageClick()
    {
        Debug.Log("Retour Clicked");
        SceneManager.LoadScene("StartingScene");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }
} 
