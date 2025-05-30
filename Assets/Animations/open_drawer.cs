using UnityEngine;

public class open_drawer : MonoBehaviour
{
    [Header("Drawer Settings")]
    public Animator animator;
    public Transform player;
    public float interactionDistance = 3f;
    public GameObject parentInteractionPrompt;
    public Outline outlineScript;

    [Header("Child Clue Settings")]
    public Outline childOutline;
    public float clueInteractionDistance = 2f;
    public GameObject childInteractionPrompt;

    private bool isOpened = false;
    public bool enablePickUpPrompt = false;

    void Awake()
    {
        animator ??= GetComponent<Animator>();
        player ??= GameObject.FindGameObjectWithTag("Player")?.transform;
        outlineScript ??= GetComponentInChildren<Outline>();
        if (childOutline == null)
            childOutline = transform.Find("Touchable2")?.GetComponent<Outline>();
    }

    void Update()
    {
        if (player == null) return;

        float distanceToDrawer = Vector3.Distance(player.position, transform.position);

        // Handle Drawer Logic
        if (!isOpened)
        {
            bool inRange = distanceToDrawer <= interactionDistance;

            outlineScript.ShowOutline(inRange);
            parentInteractionPrompt?.SetActive(inRange);

            if (inRange && Input.GetKeyDown(KeyCode.E))
            {
                animator?.SetTrigger("OPEN");
                isOpened = true;
                parentInteractionPrompt?.SetActive(false);
                outlineScript.ShowOutline(false);
                enablePickUpPrompt = true;
            }
        }
        else
        {
            parentInteractionPrompt?.SetActive(false);
            outlineScript.ShowOutline(false);
        }

        // Handle Clue Logic (only if drawer is opened)
        if (isOpened && childOutline != null)
        {
            float distanceToClue = Vector3.Distance(player.position, childOutline.transform.position);
            bool inClueRange = distanceToClue <= childOutline.maxVisibleDistance;

            childOutline.enabled = inClueRange;
            childInteractionPrompt?.SetActive(inClueRange);
        }
    }
}
