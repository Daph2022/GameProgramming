using UnityEngine;

public class ResetScale : MonoBehaviour
{
    public void ResetAllScales()
    {
        foreach (Transform child in transform)
        {
            child.localScale = Vector3.one;
        }
    }
}
