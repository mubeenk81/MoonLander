using UnityEngine;
using TMPro;

public class LandingScore : MonoBehaviour
{
    public float score = 100f;
    public float timePenaltyRate = 1f;
    public float adjustmentPenalty = 1f;
    public TextMeshProUGUI scoreText;

    private float timeElapsed = 0f;
    private bool hasLanded = false;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        UpdateScoreDisplay();
    }

    void Update()
    {
        if (hasLanded) return;

        timeElapsed += Time.deltaTime;
        score -= timePenaltyRate * Time.deltaTime;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            score -= adjustmentPenalty * Time.deltaTime;
        }

        UpdateScoreDisplay();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasLanded) return;

        if (collision.gameObject.CompareTag("LandingPad"))
        {
            // Get vertical speed (negative means falling)
            float verticalVelocity = Mathf.Abs(rb.linearVelocity.y);

            // Get angle between the contact normal and 'up' direction
            ContactPoint2D contact = collision.contacts[0];
            float contactAngle = Vector2.Angle(contact.normal, Vector2.up);

            Debug.Log($"Landing velocity: {verticalVelocity}, contact angle: {contactAngle}");

            // Define thresholds
            float maxSafeSpeed = 1f;
            float maxSafeAngle = 10f;

            if (verticalVelocity <= maxSafeSpeed && contactAngle <= maxSafeAngle)
            {
                score += 50f;
                Debug.Log("✅ Smooth landing! +50 points");
            }
            else
            {
                float speedPenalty = Mathf.Max(0f, (verticalVelocity - maxSafeSpeed) * 20f);
                float anglePenalty = Mathf.Max(0f, (contactAngle - maxSafeAngle) * 2f);
                float totalPenalty = speedPenalty + anglePenalty;

                score -= totalPenalty;
                Debug.Log($"❌ Rough landing. -{Mathf.RoundToInt(totalPenalty)} points");
            }

            // Wait briefly before determining if they bounced
            Invoke("FinalizeLanding", 0.5f);
        }
    }

    void FinalizeLanding()
    {
        // Check if still moving or bouncing (you can adjust this threshold)
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            score -= 20f;
            Debug.Log("❌ Bounce detected after landing. -20 points");
        }

        hasLanded = true;
        UpdateScoreDisplay();
    }




    // ✅ This method must be present
    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(score);
        }
    }
}
