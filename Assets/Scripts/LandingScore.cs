using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // For optional scene loading

public class LandingScore : MonoBehaviour
{
    public float score = 100f;
    public float timePenaltyRate = 1f;
    public float adjustmentPenalty = 0.5f;
    public TextMeshProUGUI scoreText;
    public GameObject endScreenPanel;
    public TextMeshProUGUI finalScoreText;
    public TextMeshProUGUI highScoreText;


    private float timeElapsed = 0f;
    private bool hasLanded = false;

    private Rigidbody2D rb;

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "GameScene") return;

        rb = GetComponent<Rigidbody2D>();
        UpdateScoreDisplay();

        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        Debug.Log("🧠 Loaded high score: " + highScore);

        if (highScoreText != null)
        {
            highScoreText.text = "High Score: " + highScore;
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "GameScene") return;
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
            float verticalVelocity = Mathf.Abs(rb.linearVelocity.y);
            ContactPoint2D contact = collision.contacts[0];
            float contactAngle = Vector2.Angle(contact.normal, Vector2.up);

            Debug.Log($"Landing velocity: {verticalVelocity}, contact angle: {contactAngle}");

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

            // Wait a moment to check for bounce, then finalize
            Invoke("FinalizeLanding", 0.5f);
        }
    }

    void FinalizeLanding()
    {
        if (hasLanded) return; // prevent duplicate calls

        if (rb.linearVelocity.magnitude > 0.5f)
        {
            score -= 20f;
            Debug.Log("❌ Bounce detected after landing. -20 points");
        }

        hasLanded = true;
        UpdateScoreDisplay();
        EndGame(); // Freeze game or transition
    }

    void UpdateScoreDisplay()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + Mathf.RoundToInt(score);
        }
    }
    void EndGame()
    {
        Time.timeScale = 0f;

        int finalScore = Mathf.RoundToInt(score);
        finalScoreText.text = "Final Score: " + finalScore;

        // 🔐 Save high score if it's a new record
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);
        if (finalScore > currentHighScore)
        {
            PlayerPrefs.SetInt("HighScore", finalScore);
            PlayerPrefs.Save(); // 🧠 Save to disk
            Debug.Log("🎉 New high score saved: " + finalScore);
        }
    }


    public void ReloadScene()
    {
        Time.timeScale = 1f; // Unpause time
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }


}
