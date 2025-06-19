using UnityEngine;

public class clue_drawer2 : MonoBehaviour
{
    public Animator animator;
    public Transform player;
    public float interactionDistance = 3f;
    public GameObject interactionPrompt; // Reference to the "Press E" text
    public Outline_Aleks outlineScript;        // Reference to the Outline component

    private bool isOpened = false;
    private bool outlineDisabled = false;
    public Outline_Aleks childOutline; // Assign in Inspector or use GetComponentInChildren


    void Awake()
    {
        if (animator == null)
            animator = GetComponent<Animator>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;

        if (outlineScript == null)
            outlineScript = GetComponentInChildren<Outline_Aleks>();
        
        if (childOutline == null)
            childOutline = transform.Find("Clue2")?.GetComponent<Outline_Aleks>();
        
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(player.position, transform.position);

        if (!isOpened && distance <= interactionDistance)
        {
            interactionPrompt?.SetActive(true);

            if (Input.GetKeyDown(KeyCode.E))
            {
                animator?.SetTrigger("OPEN2");
                isOpened = true;
                interactionPrompt?.SetActive(false);
            }
        }
        else
        {
            interactionPrompt?.SetActive(false);
        }

        // Handle outline disabling/enabling based on animation state
        if (isOpened)
        {

            
                // Only do this once
                if (!outlineDisabled)
                {
                    outlineScript.enabled = false;
                    outlineDisabled = true;
                }

                // Enable child outline once drawer is fully opened
                if (childOutline != null && !childOutline.enabled)
                {
                    childOutline.enabled = true;
                }
            
        }
    }

}