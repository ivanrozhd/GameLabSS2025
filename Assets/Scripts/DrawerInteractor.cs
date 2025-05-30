using UnityEngine;
using UnityEngine.UI; // For UI
using TMPro;          // If using TextMeshPro (optional)

public class DrawerInteractor : MonoBehaviour
{
    public Transform player;
    public GameObject interactPrompt;     // "Press E to interact"
    public GameObject escHintText;        // "Press Esc to return"
    public GameObject playerMovementRoot; // GameObject with the movement script
    public float interactDistance = 2f;
    public open_drawer open_drawer_script;
    private TouchableObject touchable;
    private bool isDisplayed = false;
    

    void Start()
    {
        touchable = GetComponent<TouchableObject>();
        interactPrompt.SetActive(false);
        escHintText.SetActive(false);
        touchable.UnTouch();
    }

    void Update()
    {

        
            if (open_drawer_script.enablePickUpPrompt)
            {
                float distance = Vector3.Distance(player.position, transform.position);

                if (distance <= interactDistance && !isDisplayed)
                {
                    interactPrompt.SetActive(true);
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        touchable.OnTouch();
                        interactPrompt.SetActive(false);
                        escHintText.SetActive(true);
                        isDisplayed = true;

                        // Freeze player movement
                        if (playerMovementRoot != null) // The NAME OF THE PLAYER CONTROLLER SCRIPT!!!!!
                            playerMovementRoot.GetComponent<FirstPersonController>().enabled = false;
                    }
                }

                // Close on Esc
                if (isDisplayed && Input.GetKeyDown(KeyCode.Escape))
                {
                    touchable.UnTouch();
                    interactPrompt.SetActive(true);
                    escHintText.SetActive(false);
                    isDisplayed = false;

                    // Re-enable movement
                    if (playerMovementRoot != null)
                        playerMovementRoot.GetComponent<FirstPersonController>().enabled = true;
                }

                // Exit zone check
                if (distance > interactDistance && isDisplayed)
                {
                    touchable.UnTouch();
                    interactPrompt.SetActive(false);
                    escHintText.SetActive(false);
                    isDisplayed = false;

                    if (playerMovementRoot != null)
                        playerMovementRoot.GetComponent<FirstPersonController>().enabled = true;
                }

                if (distance > interactDistance && !isDisplayed)
                {
                    interactPrompt.SetActive(false);
                }
            }
    }
}
