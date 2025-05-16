using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void NouvellePartieClick()
    {
        Debug.Log("Nouvelle Partie Clicked");
        SceneManager.LoadScene("Level1");
    }

    public void NiveauxClick()
    {
        Debug.Log("Niveaux Clicked");
        SceneManager.LoadScene("LevelChoiceScene");
    }

    public void RèglesClick()
    {
        Debug.Log("Règles Clicked");
        SceneManager.LoadScene("RulesScene");
    }

    public void CréditsClick()
    {
        Debug.Log("Crédits Clicked");
        SceneManager.LoadScene("CreditsScene");
    }
    public void OnExitClick()
    {
#if UNITY_EDITOR
UnityEditor.EditorApplication.isPlaying = false;
#endif 
        Application.Quit();
    }
}
