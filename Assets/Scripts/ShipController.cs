using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool shouldApplyForce;

    public float rotationSpeed = 200f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shouldApplyForce = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shouldApplyForce = true;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            shouldApplyForce = false;
        }

        float rotationInput = Input.GetAxis("Horizontal");
        RotateShip(rotationInput);
    }

    private void FixedUpdate()
    {
        if (shouldApplyForce)
        {
            rb.AddForce(transform.up * 0.3f, ForceMode2D.Force);
        }
    }

    private void RotateShip(float rotationInput)
    {
        float rotationAmount = -rotationInput * rotationSpeed * Time.deltaTime;
        rb.rotation += rotationAmount;
    }
}



