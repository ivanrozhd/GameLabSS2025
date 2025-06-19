using UnityEngine;
using System.Collections;


public class SelfDestroy : MonoBehaviour 
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public float timeForDestruction;
    
    void Start()
    {
        StartCoroutine(DestroySelf(timeForDestruction));
    }

    private IEnumerator DestroySelf(float timeForDestruction)
    {
        yield return new WaitForSeconds(timeForDestruction);
        Destroy(gameObject);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
