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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            applyThrust = true;
            var emission = thruster.emission;
            emission.enabled = true;
            if (!audioThrust.isPlaying) audioThrust.Play();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            applyThrust = false;
            var emission = thruster.emission;
            emission.enabled = false;
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
        if (collision.otherCollider.GetType() == typeof(BoxCollider2D)) {
            if (Mathf.Abs(ship.transform.rotation.z) > 0.01) shipCrashes();
            else
                //COMPRUEBA VELOCIDADES HORIZONTAL Y VERTICAL. SI ESTAN BIEN, land(), SI NO, CRASH();
                shipLands();
        }        
        else
        {
            shipCrashes();
        }
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