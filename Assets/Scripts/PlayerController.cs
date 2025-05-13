using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float rotationSpeed = 200f;
    public float thrustForce = 5f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireRate = 0.25f;

    private Rigidbody2D rb;
    private float nextFireTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;   // No gravity for space physics
        rb.linearDamping = 0.5f;         // Optional: slight drag for control
    }

    void Update()
    {
        HandleRotation();
        HandleShooting();
    }

    void FixedUpdate()
    {
        HandleThrust();
    }

    void HandleRotation()
    {
        float rotationInput = -Input.GetAxis("Horizontal");  // A/D or Left/Right
        transform.Rotate(0f, 0f, rotationInput * rotationSpeed * Time.deltaTime);
    }

    void HandleThrust()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            rb.AddForce(transform.right * thrustForce);
        }
    }

    void HandleShooting()
    {
        if (bulletPrefab == null || firePoint == null) return;

        if (Input.GetKey(KeyCode.Space) && Time.time >= nextFireTime)
        {
            Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            nextFireTime = Time.time + fireRate;
        }
    }
}
