using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

using Cinemachine;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D ship;
    private int frameCount;
    private EdgeCollider2D landingGearCollider;
    public float rotationSpeed;
    public ParticleSystem thruster;
    private bool applyThrust;
    public AudioClip crash;
    public AudioClip thrust;
    public AudioClip landed;
    AudioSource audioCrash;
    AudioSource audioThrust;
    AudioSource audioLanded;
    private bool rotationOK;
    private bool xVelocityOK;
    private bool yVelocityOK;
    private bool isCollision = false;
    public TextMeshProUGUI endReasonText;


    public GameObject brokenPartsPrefab; // Assigned in the Inspector
    public float brokenPartsForce; // Assigned in the Inspector

    public TextMeshProUGUI speedDisplay; // UI Text to display speed
    public CinemachineVirtualCamera zoomedCamera; // Reference to the zoomed camera


    private void Start()
    {
        frameCount = 0;
        ship = GetComponent<Rigidbody2D>();
        landingGearCollider = GetComponent<EdgeCollider2D>();
        thruster = GetComponentInChildren<ParticleSystem>();
        var emission = thruster.emission;
        emission.enabled = false;
        applyThrust = false;
        isCollision = false;
        rotationSpeed = 200f;

        audioCrash = gameObject.AddComponent<AudioSource>();
        audioCrash.clip = crash;
        audioThrust = gameObject.AddComponent<AudioSource>();
        audioThrust.clip = thrust;
        audioLanded = gameObject.AddComponent<AudioSource>();
        audioLanded.clip = landed;

        // Let's set an initial x speed
        Vector2 initialVelocity = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), 0);
        ship.velocity = initialVelocity;
        // This doesn't work --> ship.velocity.Set(0.06f, 0f);

        endReasonText.enabled = false;

        // Let's check the speed in the Console, every second
        //InvokeRepeating("LogEverySecond", 0f, 1f);
    }

    private bool canThrust = true;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && canThrust && Fuel > 0)
        {
            applyThrust = true;
            var emission = thruster.emission;
            emission.enabled = true;
            if (!audioThrust.isPlaying)
            {
                audioThrust.loop = true;
                audioThrust.Play();
            }
        }
        else if (Input.GetKeyUp(KeyCode.W) || Fuel <= 0)
        {
            applyThrust = false;
            var emission = thruster.emission;
            emission.enabled = false;
            audioThrust.loop = false;
            audioThrust.Stop();
        }

        float rotationInput = Input.GetAxis("Horizontal");
        RotateShip(rotationInput);

    }

    private void FixedUpdate()
    {
        // Increment the frame count
        frameCount++;

        if (applyThrust)
        {
            ship.AddForce(ship.transform.up * 0.3f, ForceMode2D.Force);
            ship.AddForce(ship.transform.up * 0.3f, ForceMode2D.Force);
            Fuel -= 1; // Reducir el combustible
            if (Fuel <= 0)
            {
                Fuel = 0;
                canThrust = false;
            }
        }

        // Check if a number of frames have passed
        if (isCollision)
        {
            Debug.Log("---------------------------------------------------------------------------------------------------");
            Debug.Log("Framecount: " + frameCount);
            Debug.Log("V: " + Mathf.Abs(ship.velocity.y) + "\nH: " + Mathf.Abs(ship.velocity.x));
        }

        // Update the speed display UI only if zoomed camera is active
        if (speedDisplay != null)
        {
            if (zoomedCamera.Priority == 10)
            {
                speedDisplay.gameObject.SetActive(true);
                // Update the speed text
                speedDisplay.text = $"H = {Mathf.Abs(ship.velocity.x):0.000}\nV = {Mathf.Abs(ship.velocity.y):0.000}\nFuel = {Fuel}";
            }
            else
            {
                speedDisplay.gameObject.SetActive(true);
            }

            
        }

        // Ensure speed text does not rotate with the ship
        if (speedDisplay != null)
        {
            speedDisplay.rectTransform.rotation = Quaternion.identity;
        }

        rotationOK = (Mathf.Abs(ship.transform.rotation.z) < 0.02);
        xVelocityOK = Mathf.Abs(ship.velocity.x) < 0.01f;
        yVelocityOK = Mathf.Abs(ship.velocity.y) < 0.04f;
    }

    private void RotateShip(float rotationInput)
    {
        float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
        ship.rotation += rotationAmount;
    }

    private bool hasCrashed = false;
    public int Fuel = 1000; // Contador de combustible

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the ship lands with the landing gear,
        // and it's straight 
        // and it's moving horizontally and vertically slow
        // then the landing is right

        if (hasCrashed) return;

        isCollision = true;

        if (collision.otherCollider.GetType() == typeof(BoxCollider2D))
        {
            if (rotationOK)
            {
                if (xVelocityOK && yVelocityOK)
                {
                    shipLands();
                    endReasonText.enabled = true;
                    endReasonText.text = "Well done!";
                    endReasonText.verticalAlignment = VerticalAlignmentOptions.Top;
                }

                else if (xVelocityOK && !yVelocityOK)
                {
                    shipCrashes();
                    endReasonText.enabled = true;
                    endReasonText.text = "Too much Vertical Speed";
                
                }
                else if (!xVelocityOK && yVelocityOK)
                {
                    shipCrashes();
                    endReasonText.enabled = true;
                    endReasonText.text = "Too much Horizontal Speed";
                }

                else if (!xVelocityOK && !yVelocityOK)
                {
                    shipCrashes();
                    endReasonText.enabled = true;
                    endReasonText.text = "You landed too hard";
                }
            }
            else
            {
                shipCrashes();
                endReasonText.enabled = true;
                endReasonText.text = "Not straight";
            }
        }
        else
        {
            shipCrashes();
            endReasonText.enabled = true;
            endReasonText.text = "Please use landing gear";
        }


        isCollision = false;
        hasCrashed = true;
    }

    private IEnumerator SetBrokenPartRotations(GameObject brokenParts)
    {
        yield return new WaitForSeconds(0.1f); // Wait for 0.1 seconds

        foreach (Transform part in brokenParts.transform)
        {
            Rigidbody2D partRigidbody = part.GetComponent<Rigidbody2D>();

            // Add a random z-axis rotation to the part
            float randomZRotation = UnityEngine.Random.Range(-180f, 180f);
            part.localRotation = Quaternion.Euler(0f, 0f, randomZRotation);

            // Add a constant torque to the part
            float torque = UnityEngine.Random.Range(-10f, 10f);
            partRigidbody.AddTorque(torque, ForceMode2D.Impulse);
        }
    }

    private void shipCrashes()
    {
        // Instantiate and activate (because it's inactive at the beginning) broken parts prefab at ship position
        GameObject brokenParts = Instantiate(brokenPartsPrefab, transform.position, Quaternion.identity);
        brokenParts.SetActive(true);

        // Loop through each broken part and apply a force in random direction
        foreach (Transform part in brokenParts.transform)
        {
            Rigidbody2D partRigidbody = part.GetComponent<Rigidbody2D>();
            Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            partRigidbody.AddForce(randomDirection * brokenPartsForce, ForceMode2D.Impulse);
        }

        disableShip();

        // Play explosion sound
        if (!audioCrash.isPlaying)
            // let's lower the audio clip's volume
            audioCrash.volume = 0.3f;
        audioCrash.Play();


        // Set the broken part rotations and add torque to each part
        StartCoroutine(SetBrokenPartRotations(brokenParts));

        StartCoroutine(RestartSceneAfterDelay(2f));
    }

    // Method to restart the scene after a delay
    private IEnumerator RestartSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }


    private void shipLands()
    {
        if (!audioLanded.isPlaying)
            audioLanded.Play();

        StartCoroutine(RestartSceneAfterDelay(2f));
    }

    private void disableShip()
    {
        SpriteRenderer shipSprite = GetComponent<SpriteRenderer>();
        shipSprite.enabled = false;

        Rigidbody2D shipRigidbody = GetComponent<Rigidbody2D>();
        // If I try to stop simulating the ship, then the virtual camera would switch to the overall camera
        ship.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        // disable the thrust sound and the particle system, just in case the player tries to thrust
        var emission = thruster.emission;
        emission.enabled = false;
        audioThrust.Stop();
        // flag to disable the thrust key
        canThrust = false;
        // get the damn ship to stop if it touched ground
        shipRigidbody.isKinematic = true;
    }

    /*    void LogEverySecond()
        {
            Debug.Log("Frame count: " + frameCount + "Vertical Speed: " + Mathf.Abs(ship.velocity.y) + ", Horizontal Speed: " + Mathf.Abs(ship.velocity.x));
        }*/
}
