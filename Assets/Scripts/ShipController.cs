using System;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D ship;
    //private PolygonCollider2D bodyCollider;
    //private BoxCollider2D leftLandingGearCollider;
    //private CapsuleCollider2D rightLandingGearCollider;
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


    private void Start()
    {
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
    }

    private void Update()
    {
        Debug.Log("Vertical Speed: " + Mathf.Abs(ship.velocity.y) + "Horizontal Speed: " + Mathf.Abs(ship.velocity.x));

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
        if (applyThrust)
        {
            ship.AddForce(transform.up * 0.3f, ForceMode2D.Force);
        }
    }

    private void RotateShip(float rotationInput)
    {
        float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
        ship.rotation += rotationAmount;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // If the ship lands with the landing gear
        if (collision.otherCollider.GetType() == typeof(BoxCollider2D))
        {
            // If it's straight
            if (Mathf.Abs(ship.transform.rotation.z) < 0.02)
            {
                // if the speed is ok
                if (Mathf.Abs(ship.velocity.x) < 0.015f && Mathf.Abs(ship.velocity.y) < 0.05f)
                {
                    shipLands();
                }
                else shipCrashes();
            }
            else shipCrashes();
        }
        else shipCrashes();
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
}