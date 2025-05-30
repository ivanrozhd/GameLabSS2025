// Script by Marcelli Michele

using System.Linq;
using UnityEngine;

public class PadLockPassword : MonoBehaviour
{
    MoveRuller _moveRull;

    public int[] _numberPassword = {0,0,0,0};
    
    public bool passWordCorrect = false;

    private void Awake()
    {
        _moveRull = FindObjectOfType<MoveRuller>();
    }

    public void Password()
    {
        if (_moveRull._numberArray.SequenceEqual(_numberPassword) && Input.GetKeyDown(KeyCode.Return))
        {
            // Here enter the event for the correct combination
            Debug.Log("Password correct");

            // Es. Below the for loop to disable Blinking Material after the correct password
            for (int i = 0; i < _moveRull._rullers.Count; i++)
            {
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                _moveRull._rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
            }
            
            passWordCorrect = true;

        }

        if (!_moveRull._numberArray.SequenceEqual(_numberPassword) && Input.GetKeyDown(KeyCode.Return))
        {
            Debug.Log("Password incorrect");

            foreach (GameObject ruler in _moveRull._rullers)
            {
                ruler.GetComponent<PadLockEmissionColor>().FlashRed();
            }
        }
    }
}
