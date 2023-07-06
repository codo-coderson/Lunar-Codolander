using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D ship;
    private PolygonCollider2D bodyCollider;
    public float rotationSpeed;

    public ParticleSystem thruster;
    private bool shouldApplyForce;
    public bool bang;



    public BoxCollider2D sideOfleftLeg;
    public BoxCollider2D landingGear;
    public BoxCollider2D sideOfRightLeg;

    private void Start()
    {
        ship = GetComponent<Rigidbody2D>();
        bodyCollider = GetComponent<PolygonCollider2D>();
        thruster = GetComponentInChildren<ParticleSystem>();
        var emission = thruster.emission;
        emission.enabled = false;        
        shouldApplyForce = false;
        bang = false;
        rotationSpeed = 200f;

        sideOfleftLeg = GetComponent<BoxCollider2D>();
        landingGear = GetComponent<BoxCollider2D>();
        sideOfRightLeg = GetComponent<BoxCollider2D>();
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldApplyForce = true;
            var emission = thruster.emission;
            emission.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            shouldApplyForce = false;
            var emission = thruster.emission;
            emission.enabled = false;
        }

        float rotationInput = Input.GetAxis("Horizontal");
        RotateShip(rotationInput);
    }

    private void FixedUpdate()
    {
        if (shouldApplyForce)
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

        if (collision.collider == landingGear)
        {
            Debug.Log("Landed with landing gear!");
        }
        else
        {
            Debug.Log("NOT landed with landing gear!");
        }

        // condition: the ship has to land straight
        bool notRotated = Mathf.Abs(ship.transform.rotation.z) < 0.01;
                
        if (notRotated)
        {
            bang = false;
            //Debug.Log("Z: " + Mathf.Abs(ship.transform.rotation.z));
            Debug.Log("Bang: " + bang);

        }
        else
        {
            bang = true;
            //Debug.Log("Z: " + Mathf.Abs(ship.transform.rotation.z));
            Debug.Log("Bang: " + bang);
        }
    }
}