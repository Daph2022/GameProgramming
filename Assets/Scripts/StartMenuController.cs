using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnStartClick()
    {
        Debug.Log("Start Button Clicked");
        SceneManager.LoadScene("Level1");
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }
}
