using System;
using UnityEngine;

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

        // Let's set an initial x speed, currently for debugging purposes
        Vector2 initialVelocity = new Vector2(0f, 0f);
        ship.velocity = initialVelocity;
        // This doesn't work --> ship.velocity.Set(0.06f, 0f);


        // Let's check the speed in the Console, every second
        //InvokeRepeating("LogEverySecond", 0f, 1f);
    }

    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.W))
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
        else if (Input.GetKeyUp(KeyCode.W))
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
            ship.AddForce(transform.up * 0.3f, ForceMode2D.Force);
        }

        //Check if a number of frames have passed
        if (isCollision)
        {
            Debug.Log("---------------------------------------------------------------------------------------------------");
            Debug.Log("Framecount: " + frameCount);
            Debug.Log("Vertical Speed: " + Mathf.Abs(ship.velocity.y) + ", Horizontal Speed: " + Mathf.Abs(ship.velocity.x));
        }

        rotationOK = (Mathf.Abs(ship.transform.rotation.z) < 0.02);
        xVelocityOK = Mathf.Abs(ship.velocity.x) < 0.0008f;
        yVelocityOK = Mathf.Abs(ship.velocity.y) < 0.04f;
    }

    private void RotateShip(float rotationInput)
    {
        float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
        ship.rotation += rotationAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // If the ship lands with the landing gear,
        // and it's straight 
        // and it's moving horizontally and vertically slow
        // then the landing is right

        isCollision = true;

        if (collision.otherCollider.GetType() == typeof(BoxCollider2D))
        {
            if (rotationOK)
            {
                if (xVelocityOK)
                {
                    if (yVelocityOK)
                    {
                        shipLands();                        
                    }
                    else
                    {
                        shipCrashes();
                        Debug.Log("Reason of crash: vertical speed");
                    }
                }
                else
                {
                    shipCrashes();
                    Debug.Log("Reason of crash: horizontal speed");
                }
            }
            else
            {
                shipCrashes();
                Debug.Log("Reason of crash: not straight");
            }
        }
        else
        {
            shipCrashes();
            Debug.Log("Reason of crash: not landed with landing gear");
        }

        isCollision = false;
    }

    private void shipCrashes()
    {
        if (!audioCrash.isPlaying)
            audioCrash.Play();
    }

    private void shipLands()
    {
        if (!audioLanded.isPlaying)
            audioLanded.Play();
    }


/*    void LogEverySecond()
    {
        Debug.Log("Frame count: " + frameCount + "Vertical Speed: " + Mathf.Abs(ship.velocity.y) + ", Horizontal Speed: " + Mathf.Abs(ship.velocity.x));
    }*/
}
