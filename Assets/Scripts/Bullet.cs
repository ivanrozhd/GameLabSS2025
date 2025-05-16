using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Bullet : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Target"))
        {
            CreateBulletImpactEffect(collision);
            print("hit " + collision.gameObject.name + "!");
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("Wall"))
        {
            CreateBulletImpactEffect(collision);
            print("hit a wall!");
            Destroy(gameObject);
        }
        
        if (collision.gameObject.CompareTag("BeerBottle"))
        {
            //CreateBulletImpactEffect(collision);
            print("hit a beer bottle!");
            BeerBottle bottle = collision.gameObject.GetComponent<BeerBottle>();
            if (bottle != null)
            {
                bottle.Shatter();
            }           // Destroy(gameObject); dont destroy the bullet when it hits the bottle
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];
        GameObject hole = Instantiate(GlobalReferences.Instance.bulletImpactEffectPrefab, contact.point, Quaternion.LookRotation(contact.normal));

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }
}
