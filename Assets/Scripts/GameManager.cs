using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance; // Singleton instance

    public int lives = 3; // Initial number of lives

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist this object between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoseLife()
    {
        int previousLives = lives;
        lives--;
        Debug.Log($"Lives before crash: {previousLives}. Remaining lives: {lives}");

        if (lives <= 0)
        {
            StartCoroutine(GameOverCoroutine());
        }
        else
        {
            StartCoroutine(ReloadLevelAfterDelay(2f)); // Wait 2 seconds before reloading
        }
    }

    public void LevelComplete()
    {
        StartCoroutine(LevelCompleteCoroutine());
    }

    private IEnumerator LevelCompleteCoroutine()
    {
        yield return new WaitForSeconds(2f);

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int totalScenes = SceneManager.sceneCountInBuildSettings;

        if (currentSceneIndex + 1 < totalScenes)
        {
            // Load the next level
            SceneManager.LoadScene(currentSceneIndex + 1);
        }
        else
        {
            // No more levels, game over (player wins)
            Debug.Log("Congratulations! You've completed all levels.");
            StartCoroutine(GameOverCoroutine(true));
        }
    }

    private IEnumerator ReloadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for specified delay
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator GameOverCoroutine(bool playerWon = false)
    {
        yield return new WaitForSeconds(2f); // Wait 2 seconds before proceeding

        if (playerWon)
        {
            // Handle player winning the game (e.g., show victory screen)
            Debug.Log("Player has won the game!");
            // Load a victory scene or main menu
            SceneManager.LoadScene("LanderMainMenu");
        }
        else
        {
            // Handle game over due to losing all lives
            Debug.Log("Game Over. You've lost all your lives.");
            ResetLives(); // Reset lives for next game
            SceneManager.LoadScene("LanderMainMenu");
        }
    }

    public void ResetLives()
    {
        lives = 3; // Reset lives to initial value
    }
}
