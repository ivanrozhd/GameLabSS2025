using TMPro;
using UnityEngine;

public class TextColorChanger : MonoBehaviour
{
    public TextMeshProUGUI text;

    void Start()
    {
        // This just changes the color property, not the material
        text.color = Color.black;
    }
}
