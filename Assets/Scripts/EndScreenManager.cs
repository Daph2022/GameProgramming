using UnityEngine;

public class EndScreenManager : MonoBehaviour
{
    public void ReturnToMenu()
    {
        SceneController.instance.LoadMainMenu();
    }
}
