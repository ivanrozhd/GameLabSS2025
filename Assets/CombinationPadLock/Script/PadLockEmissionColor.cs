// Script by Marcelli Michele

using UnityEngine;

using UnityEngine;

public class PadLockEmissionColor : MonoBehaviour
{
    TimeBlinking tb;

    private GameObject _myRuller;
    private Color customColor = new Color(1f,0.8196079f,0.2862745f,0.1f);
    private Color redFlashColor = Color.red;

    [HideInInspector]
    public bool _isSelect;

    private void Awake()
    {
        tb = FindObjectOfType<TimeBlinking>();
    }

    void Start()
    {
        _myRuller = gameObject;
    }

    public void BlinkingMaterial()
    {
        _myRuller.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");

        if (_isSelect)
        {
            _myRuller.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.Lerp(Color.clear, customColor, Mathf.PingPong(Time.time, tb.blinkingTime)));
        }
        else
        {
            _myRuller.GetComponent<Renderer>().material.SetColor("_EmissionColor", Color.clear);
        }
    }

    // Blink red once
    public void FlashRed()
    {
        StopAllCoroutines(); // Cancel any ongoing emission changes
        StartCoroutine(RedFlashCoroutine());
    }

    private System.Collections.IEnumerator RedFlashCoroutine()
    {
        Renderer rend = _myRuller.GetComponent<Renderer>();

        // Temporarily stop blinking
        bool prevSelect = _isSelect;
        _isSelect = false;
        rend.material.EnableKeyword("_EMISSION");

        // Set red color
        rend.material.SetColor("_EmissionColor", redFlashColor);

        yield return new WaitForSeconds(0.6f); // Flash duration (increase this for longer)

        // Clear emission color
        rend.material.SetColor("_EmissionColor", Color.clear);

        // Resume previous blinking state
        _isSelect = prevSelect;
    }
}

