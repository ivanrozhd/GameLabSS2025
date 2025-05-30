using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    // Name of the scene to load, set in Inspector or here:
    [SerializeField] private string sceneToLoad = "Outside";
    
    
    
    public GameObject playerMovementRoot; // gameobject with player movementscript
    
    public void LoadNextScene()
    {
        Debug.Log("LoadNextScene called!");
        if (playerMovementRoot != null)
        {
            Debug.Log("Disabling player movement");
            playerMovementRoot.GetComponent<FirstPersonController>().enabled = false;
        }
        else
        {
            Debug.LogWarning("playerMovementRoot is null!");
        }
    
        SceneManager.LoadScene(sceneToLoad);
    }
}
