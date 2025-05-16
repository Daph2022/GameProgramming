using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public bool isFinalPoint = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (isFinalPoint)
            {
                SceneController.instance.LoadScene("EndScene");
            }
            else
            {
                SceneController.instance.NextLevel();
            }
        }
    }
}
