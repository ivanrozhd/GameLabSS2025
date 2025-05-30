using UnityEngine;
using UnityEngine.SceneManagement;

public class PuzzleSceneManager : MonoBehaviour
{
    [SerializeField] private GameObject spaceToContinuePrompt;  // Assign UI Text in Inspector
    [SerializeField] private string sceneToLoad = "Outside";

    public GameObject playerMovementRoot; // Assign player root with movement script

    private bool puzzleSolved = false;
    private bool readyToContinue = false;

    void Update()
    {
        if (puzzleSolved && !readyToContinue)
        {
            // Show prompt once puzzle is solved
            spaceToContinuePrompt.SetActive(true);
            readyToContinue = true;

            // Freeze player movement
            if (playerMovementRoot != null)
            {
                var controller = playerMovementRoot.GetComponent<FirstPersonController>();
                if (controller != null)
                    controller.enabled = false;
            }
        }

        if (readyToContinue && Input.GetKeyDown(KeyCode.Space))
        {
            LoadNextScene();
        }
    }

    // Call this method when your puzzle is solved
    public void OnPuzzleSolved()
    {
        puzzleSolved = true;
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}