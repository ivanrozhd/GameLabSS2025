using UnityEngine;

public class TouchableObject : MonoBehaviour
{
    public GameObject displayImage; // Assign BookDisplayUI here

    public void OnTouch()
    {
        displayImage.SetActive(true);
    }

    public void UnTouch()
    {
        displayImage.SetActive(false);
    }
}
