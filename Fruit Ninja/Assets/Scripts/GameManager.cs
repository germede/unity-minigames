using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public class GameManager : MonoBehaviour {

    [Header("Score Elements")]
    public int score;
    public int highScore;
    public Text scoreText;
    public Text highscoreText;

    [Header("GameOver")]
    public GameObject gameOverPanel;
    public Text gameOverPanelScoreText;
    public Text gameOverPanelHighscoreText;
    public Button restartBtn;

    private void Awake() {
        Advertisement.Initialize("1581415");

        gameOverPanel.SetActive(false);
        GetHighscore();
    }

    public void IncreaseScore(int v) {
        score += v;
        scoreText.text = score.ToString();

        if(score > highScore) {
            PlayerPrefs.SetInt("Highscore", score);
            highscoreText.text = "Best: " + score;
        }
    }

    public void OnBombHit() {
        Advertisement.Show();

        Time.timeScale = 0;

        gameOverPanelScoreText.text = "Score: " + score.ToString();
        gameOverPanelHighscoreText.text = "Best: " + highScore.ToString();

        gameOverPanel.SetActive(true);
        Debug.Log("Bomb hitted");
    }

    private void GetHighscore() {
        highScore = PlayerPrefs.GetInt("Highscore");
        highscoreText.text = "Best: " + highScore;
    }

    public void RestartGame() {
        GetHighscore();

        // Reset the score and the game over panel
        score = 0;
        scoreText.text = score.ToString();

        gameOverPanel.SetActive(false);

        // Find gameobjects to destroy
        foreach(GameObject g in GameObject.FindGameObjectsWithTag("Interactable"))
            Destroy(g);

        Time.timeScale = 1;
    }
    
}
