using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerControllerX : MonoBehaviour
{
    private Rigidbody playerRb;
    private float speed = 500;
    private GameObject focalPoint;
    private float turboStartTime;

    public GameObject smokeObj;
    public bool hasTurbo;
    public bool hasPowerup;
    public GameObject powerupIndicator;
    public int powerUpDuration = 5;
    public float turboSpeed;

    private float normalStrength = 10; // how hard to hit enemy without powerup
    private float powerupStrength = 25; // how hard to hit enemy with powerup
    
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
    }

    void Update()
    {
        // Add force to player in direction of the focal point (and camera)
        float verticalInput = Input.GetAxis("Vertical");

        if (hasTurbo)
        {
            playerRb.AddForce(focalPoint.transform.forward * verticalInput * turboSpeed * Time.deltaTime);
            //y = 10 - 2.5x   for speed = 5
            turboSpeed = speed * 2 - speed/2 * (Time.time - turboStartTime);
            if (turboSpeed<speed)
            {
                hasTurbo = false;
            }
        }
        else
            playerRb.AddForce(focalPoint.transform.forward * verticalInput * speed * Time.deltaTime);

        if (Input.GetKeyDown(KeyCode.Space) && !hasTurbo)
        {
            hasTurbo = true;
            turboSpeed = speed * 2;
            turboStartTime = Time.time;
            
            smokeObj.gameObject.GetComponent<ParticleSystem>().Play();
        }

        // Set powerup indicator position to beneath player
        Vector3 position = transform.position;
        powerupIndicator.transform.position = position + new Vector3(0, -0.6f, 0);
        smokeObj.transform.position = position;
        
        Quaternion rotation = transform.rotation;
        smokeObj.transform.rotation = new Quaternion(rotation.x, rotation.y+90, rotation.z, rotation.w);

    }

    // If Player collides with powerup, activate powerup
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Powerup"))
        {
            Destroy(other.gameObject);
            hasPowerup = true;
            powerupIndicator.SetActive(true);
            StartCoroutine(PowerupCooldown());
        }
    }

    // Coroutine to count down powerup duration
    IEnumerator PowerupCooldown()
    {
        yield return new WaitForSeconds(powerUpDuration);
        hasPowerup = false;
        powerupIndicator.SetActive(false);
    }

    // If Player collides with enemy
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Rigidbody enemyRigidbody = other.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromPlayer = other.gameObject.transform.position -  transform.position; 
           
            if (hasPowerup) // if have powerup hit enemy with powerup force
            {
                enemyRigidbody.AddForce(awayFromPlayer * powerupStrength, ForceMode.Impulse);
            }
            else // if no powerup, hit enemy with normal strength 
            {
                enemyRigidbody.AddForce(awayFromPlayer * normalStrength, ForceMode.Impulse);
            }


        }
    }



}
