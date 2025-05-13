using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Text scoreText;
    public GameObject gameOverPanel;

    private float timeSurvived = 0f;
    private bool isGameOver = false;

    void Update()
    {
        if (isGameOver) return;

        timeSurvived += Time.deltaTime;
        scoreText.text = "Score: " + Mathf.FloorToInt(timeSurvived);
    }

    public void GameOver()
    {
        isGameOver = true;
        gameOverPanel.SetActive(true);
    }

    public void Restart()
    {
        SceneManager.LoadScene("GameScene");
    }
}
