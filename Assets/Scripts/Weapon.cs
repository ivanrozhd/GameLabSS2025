using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    // Bullet settings
    public GameObject bulletPrefab;
    public Transform bulletSpawn;
    public float bulletSpeed = 30f;
    public float bulletDamage = 50f;
    public float bulletLifeTime = 2f;
    
    // Audio settings
    public AudioClip gunShotClip;
    public AudioSource audioSource;
    public Vector2 audioPitch = new Vector2(.9f, 1.1f);
    
    // Muzzle flash
    public GameObject muzzlePrefab;
    public GameObject muzzlePosition;
    
    // Shooting mechanics
    public Camera playerCamera;
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;
    public int bulletsPerBurst = 3;
    public int burstBulletsLeft;
    public float spreadIntensity;
    
    public enum ShootingMode { Single, Burst, Auto }
    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletsLeft = bulletsPerBurst;
        
        // Initialize audio if not set
        if (audioSource != null && gunShotClip != null)
        {
            audioSource.clip = gunShotClip;
        }
    }
    
    void Update()
    {
        if (currentShootingMode == ShootingMode.Auto)
        {
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if (currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }

        if (readyToShoot && isShooting)
        {
            burstBulletsLeft = bulletsPerBurst;
            FireWeapon();
        }
    }

    private void FireWeapon()
    {
        readyToShoot = false;
        Vector3 shootingDirection = CalculateDirectionAndSpread().normalized;
        
        // Create muzzle flash
        if (muzzlePrefab != null && muzzlePosition != null)
        {
            Instantiate(muzzlePrefab, muzzlePosition.transform.position, muzzlePosition.transform.rotation, muzzlePosition.transform);
        }
        
        // Play gunshot sound
        PlayGunshotSound();
        
        // Create bullet
        GameObject bullet = Instantiate(bulletPrefab, bulletSpawn.position, bulletSpawn.rotation);
        bullet.transform.forward = shootingDirection;
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection * bulletSpeed, ForceMode.Impulse);
        StartCoroutine(DestroyBulletAfterTime(bullet, bulletLifeTime));
        
        if (allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        if (currentShootingMode == ShootingMode.Burst && burstBulletsLeft > 1)
        {
            burstBulletsLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
    }

    private void PlayGunshotSound()
    {
        if (audioSource != null && gunShotClip != null)
        {
            if (audioSource.transform.IsChildOf(transform))
            {
                // Randomize pitch for variation
                audioSource.pitch = Random.Range(audioPitch.x, audioPitch.y);
                audioSource.PlayOneShot(gunShotClip);
            }
            else
            {
                // Create temporary audio source if needed
                AudioSource tempAudio = Instantiate(audioSource);
                tempAudio.pitch = Random.Range(audioPitch.x, audioPitch.y);
                tempAudio.PlayOneShot(gunShotClip);
                Destroy(tempAudio.gameObject, gunShotClip.length + 0.1f);
            }
        }
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }
    
    public Vector3 CalculateDirectionAndSpread()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;
        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator DestroyBulletAfterTime(GameObject bullet, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (bullet != null)
        {
            Destroy(bullet);
        }
    }
}