``` using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void CreditsPageClick()
    {
        Debug.Log("Retour Clicked");
        SceneManager.LoadScene("StartingScene");
    }
    public void Level1Click()
    {
        Debug.Log("Level 1 Clicked");
        SceneManager.LoadScene("Level1");
    }

    public void Level2Click()
    {
        Debug.Log("Level 1 Clicked");
        SceneManager.LoadScene("Level2");
    }
    public void FinalLevelClick()
    {
        Debug.Log("Level 1 Clicked");
        SceneManager.LoadScene("FinalStage");
    }
    public void OnExitClick()
    {
#if UNITY_EDITOR
UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }
} 


```