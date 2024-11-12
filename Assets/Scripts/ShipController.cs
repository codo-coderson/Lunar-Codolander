using System.Collections;
using UnityEngine;
using TMPro;
using Cinemachine;
using UnityEngine.SceneManagement; // Added namespace

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
    private bool canThrust = true;
    public int Fuel = 1000; // Fuel counter
    private bool hasCrashed = false;

    // Reference to the Cinemachine virtual camera
    public CinemachineVirtualCamera zoomedCamera; // Reference to the zoomed camera

    // Speed display UI (handled by the ship)
    public TextMeshProUGUI speedDisplay; // UI Text to display speed

    // Variables for broken parts
    public GameObject brokenPartsPrefab; // Assigned in the Inspector
    public float brokenPartsForce; // Assigned in the Inspector

    private void Start()
    {
        frameCount = 0;
        ship = GetComponent<Rigidbody2D>();
        landingGearCollider = GetComponent<EdgeCollider2D>();
        thruster = GetComponentInChildren<ParticleSystem>();
        var emission = thruster.emission;
        emission.enabled = false;
        applyThrust = false;
        rotationSpeed = 200f;

        audioCrash = gameObject.AddComponent<AudioSource>();
        audioCrash.clip = crash;
        audioThrust = gameObject.AddComponent<AudioSource>();
        audioThrust.clip = thrust;
        audioLanded = gameObject.AddComponent<AudioSource>();
        audioLanded.clip = landed;

        // Set an initial x speed
        Vector2 initialVelocity = new Vector2(UnityEngine.Random.Range(-0.5f, 0.5f), 0);
        ship.velocity = initialVelocity;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && canThrust && Fuel > 0)
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
        else if (Input.GetKeyUp(KeyCode.UpArrow) || Fuel <= 0)
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
            Fuel -= 1; // Reduce fuel
            if (Fuel <= 0)
            {
                Fuel = 0;
                canThrust = false;
            }
        }

        // Update the speed display UI
        if (speedDisplay != null)
        {
            speedDisplay.gameObject.SetActive(true);
            // Update the speed text
            speedDisplay.text = $"H = {Mathf.Abs(ship.velocity.x):0.000}\nV = {Mathf.Abs(ship.velocity.y):0.000}\nFuel = {Fuel}";
            // Ensure speed text does not rotate with the ship
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the ship has already crashed, then return
        if (hasCrashed) return;

        if (collision.otherCollider.GetType() == typeof(BoxCollider2D))
        {
            if (rotationOK)
            {
                if (xVelocityOK && yVelocityOK)
                {
                    shipLands();
                    UIManager.instance?.ShowEndReason("Well done!");
                }
                else if (xVelocityOK && !yVelocityOK)
                {
                    shipCrashes();
                    UIManager.instance?.ShowEndReason("Too much Vertical Speed");
                }
                else if (!xVelocityOK && yVelocityOK)
                {
                    shipCrashes();
                    UIManager.instance?.ShowEndReason("Too much Horizontal Speed");
                }
                else if (!xVelocityOK && !yVelocityOK)
                {
                    shipCrashes();
                    UIManager.instance?.ShowEndReason("Too fast");
                }
            }
            else
            {
                shipCrashes();
                UIManager.instance?.ShowEndReason("Not straight");
            }
        }
        else
        {
            shipCrashes();
            UIManager.instance?.ShowEndReason("Please use landing gear");
        }

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
        // Instantiate and activate broken parts prefab at ship position
        GameObject brokenParts = Instantiate(brokenPartsPrefab, transform.position, Quaternion.identity);
        brokenParts.SetActive(true);

        // Apply force to each broken part
        foreach (Transform part in brokenParts.transform)
        {
            Rigidbody2D partRigidbody = part.GetComponent<Rigidbody2D>();
            Vector2 randomDirection = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            partRigidbody.AddForce(randomDirection * brokenPartsForce, ForceMode2D.Impulse);
        }

        disableShip();

        // Play explosion sound
        if (!audioCrash.isPlaying)
            audioCrash.volume = 0.3f;
        audioCrash.Play();

        // Set the broken part rotations and add torque to each part
        StartCoroutine(SetBrokenPartRotations(brokenParts));

        // Notify GameManager to handle life loss and scene reload
        GameManager.instance.LoseLife();
    }

    private void shipLands()
    {
        if (!audioLanded.isPlaying)
            audioLanded.Play();

        // Notify GameManager to handle level completion
        GameManager.instance.LevelComplete();
    }

    private void disableShip()
    {
        SpriteRenderer shipSprite = GetComponent<SpriteRenderer>();
        shipSprite.enabled = false;

        Rigidbody2D shipRigidbody = GetComponent<Rigidbody2D>();
        // Freeze the ship's movement
        ship.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;

        // Disable thrust sound and particle system
        var emission = thruster.emission;
        emission.enabled = false;
        audioThrust.Stop();
        canThrust = false;

        // Stop physics simulation on the ship
        shipRigidbody.isKinematic = true;
    }
}
