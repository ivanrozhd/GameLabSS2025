// Script by Marcelli Michele
// adapted by Aleksandar

using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // for scene loading
using UnityEngine.UI; // for UI Button


public class MoveRuller : MonoBehaviour
{
    PadLockPassword _lockPassword;
    PadLockEmissionColor _pLockColor;
    
    [SerializeField] private TextMeshPro doorUIText; // Reference to the GameObject that holds the TextMeshPro + script

    
    [Header("Next Scene")]
    [SerializeField] private string sceneToLoad = "Outside"; // set your scene name

    [SerializeField] PuzzleSceneManager PuzzleSceneManager;
    
    
    
    
    [HideInInspector]
    public List <GameObject> _rullers = new List<GameObject>();
    private int _scroolRuller = 0;
    private int _changeRuller = 0;
    [HideInInspector]
    public int[] _numberArray = {0,0,0,0};

    private int _numberRuller = 0;

    private bool _isActveEmission = false;
    private CameraLockManager lockManager;
    
    private bool isInteracting = false;
    private bool solved = false;
    private bool destructionScheduled = false;

    
    
    
    [Header("Interaction Settings")]
    public Transform player;
    public GameObject interactPrompt;     // "Press E to interact"
    public GameObject escHintText;        // "Press Esc to return"
    public GameObject puzzleMovement;     // The instructions on how to move the puzzle locks
    public GameObject playerMovementRoot; // GameObject with the movement script
    public float interactDistance = 1.5f;


    void Awake()
    {
        _lockPassword = FindObjectOfType<PadLockPassword>();
        _pLockColor = FindObjectOfType<PadLockEmissionColor>();
        lockManager = FindObjectOfType<CameraLockManager>(); // 

        _rullers.Add(GameObject.Find("Ruller1"));
        _rullers.Add(GameObject.Find("Ruller2"));
        _rullers.Add(GameObject.Find("Ruller3"));
        _rullers.Add(GameObject.Find("Ruller4"));

        foreach (GameObject r in _rullers)
        {
            r.transform.Rotate(-144, 0, 0, Space.Self);
        }
    }
    void Update()
    {
        if (!solved)
        {
            
        float distance = Vector3.Distance(player.position, transform.position);
        
        
        if (distance <= interactDistance && !isInteracting)
        {
            interactPrompt.SetActive(true);
            
            if (Input.GetKeyDown(KeyCode.E))
            {
                isInteracting = true;
                interactPrompt.SetActive(false);
                escHintText.SetActive(true);
                puzzleMovement.SetActive(true);
                // Freeze player movement
                if (playerMovementRoot != null)
                {
                                                    // The NAME OF THE PLAYER CONTROLLER SCRIPT!!!!!
                    playerMovementRoot.GetComponent<FirstPersonController>().enabled = false;
                }

                lockManager.CameraToLock();
                
                
                
            }

        }
        
        
        
        if (isInteracting)
        {
            MoveRulles();
            RotateRullers();
            _lockPassword.Password(); // If this also needs to run continuously
        }


        if (lockManager.isCameraLocked && Input.GetKeyDown(KeyCode.Escape))
        {
            isInteracting = false;
            escHintText.SetActive(false);
            puzzleMovement.SetActive(false);
            interactPrompt.SetActive(true);
            
            // Re-enable movement
            if (playerMovementRoot != null)
                playerMovementRoot.GetComponent<FirstPersonController>().enabled = true;

            
            lockManager.CameraToPlayer();
            
        }
        
        
        // Exit zone check
        if (distance > interactDistance && lockManager.isCameraLocked)
        {
            isInteracting = false;
            interactPrompt.SetActive(false);
            escHintText.SetActive(false);
            puzzleMovement.SetActive(false);

            if (playerMovementRoot != null)
                playerMovementRoot.GetComponent<FirstPersonController>().enabled = true;
        }

        if (distance > interactDistance && !lockManager.isCameraLocked)
        {
            interactPrompt.SetActive(false);
        }



        if (_lockPassword.passWordCorrect)
        {
            destructionScheduled = true;

            EnableGravityToChildren();

            // Re-enable player movement
            if (playerMovementRoot != null)
                playerMovementRoot.GetComponent<FirstPersonController>().enabled = true;

            // Snap camera back
            lockManager.CameraToPlayer();

            // Disable prompts and hints
            escHintText.SetActive(false);
            puzzleMovement.SetActive(false);
            interactPrompt.SetActive(false);
            
            PuzzleSceneManager.OnPuzzleSolved();
            // Schedule the actual disabling of the padlock GameObject
            //Invoke(nameof(ShowNextSceneButton), 2.0f);
            //Invoke(nameof(DisablePadlock), 2.0f);
        }
        }
    }

    void MoveRulles()
    {
        if (Input.GetKeyDown(KeyCode.L)) 
        {
            _isActveEmission = true;
            _changeRuller ++;
            _numberRuller += 1;

            if (_numberRuller > 3)
            {
                _numberRuller = 0;
            }
        }
        if (Input.GetKeyDown(KeyCode.J)) 
        {
            _isActveEmission = true;
            _changeRuller --;
            _numberRuller -= 1;

            if (_numberRuller < 0)
            {
                _numberRuller = 3;
            }
        }
        _changeRuller = (_changeRuller + _rullers.Count) % _rullers.Count;


        for (int i = 0; i < _rullers.Count; i++)
        {
            if (_isActveEmission)
            {
                if (_changeRuller == i)
                {

                    _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = true;
                    _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                }
                else
                {
                    _rullers[i].GetComponent<PadLockEmissionColor>()._isSelect = false;
                    _rullers[i].GetComponent<PadLockEmissionColor>().BlinkingMaterial();
                }
            }
        }

    }

    void RotateRullers()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(-_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] += 1;

            if (_numberArray[_changeRuller] > 9)
            {
                _numberArray[_changeRuller] = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            _isActveEmission = true;
            _scroolRuller = 36;
            _rullers[_changeRuller].transform.Rotate(_scroolRuller, 0, 0, Space.Self);

            _numberArray[_changeRuller] -= 1;

            if (_numberArray[_changeRuller] < 0)
            {
                _numberArray[_changeRuller] = 9;
            }
        }
    }
    
    void EnableGravityToChildren()
    {
        string[] objectNames = {
            "Ruller1",
            "Ruller2",
            "Ruller3",
            "Ruller4",
            "PadlockSeparationCylinder",
            "PadlockCap"
        };

        foreach (string name in objectNames)
        {
            Transform child = transform.Find(name);
            if (child != null)
            {
                Rigidbody rb = child.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.useGravity = true;
                    rb.isKinematic = false;
                }
            }
        }
    }
    
    private void DisablePadlock()
    {
        if (doorUIText != null)
            doorUIText.enabled = true;
        gameObject.SetActive(false); // Disables the entire padlock GameObject
    }

    
}
