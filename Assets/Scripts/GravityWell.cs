using UnityEngine;

public class GravityWell : MonoBehaviour
{
    public float gravityStrength = 10f;

    void FixedUpdate()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (transform.position - player.transform.position).normalized;
                float distance = Vector2.Distance(transform.position, player.transform.position);
                float forceMagnitude = gravityStrength / Mathf.Clamp(distance, 1f, 10f); // Stronger when close

                rb.AddForce(direction * forceMagnitude);
            }
        }
    }
}
