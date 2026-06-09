using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Joueur")]
    public Transform player;

    public float deathY = -10f;

    [Header("UI")]
    public TextMeshProUGUI timerText;

    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI restartText;
    public TextMeshProUGUI bestTimer;

    [Header("Animation texte")]
    public float blinkSpeed = 2f;

    private float timer = 0f;
    private bool gameRunning = true;
    private bool isDead = false;

    private void Awake()
    {
        Instance = this;

        // Cache le panel au démarrage
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    private void Update()
    {
        if (!gameRunning)
        {
            // Clignotement du texte ESPACE
            if (restartText != null)
            {
                float alpha = (Mathf.Sin(Time.unscaledTime * blinkSpeed * Mathf.PI) + 1f) / 2f;
                Color c = restartText.color;
                c.a = Mathf.Lerp(0.1f, 1f, alpha);
                restartText.color = c;
            }

            if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                Restart();

            return;
        }

        // Chronomètre
        timer += Time.deltaTime;
        UpdateTimerUI();

        // Détection chute
        if (!isDead && player != null && player.position.y < deathY)
        {
            isDead = true;
            GameOver();
        }

        // Si joueur détruit
        if (!isDead && player == null)
        {
            isDead = true;
            GameOver();
        }
    }

    private void UpdateTimerUI()
    {
        if (timerText == null) return;
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int centiseconds = Mathf.FloorToInt((timer * 100f) % 100f);
        timerText.text = $"{minutes:00}:{seconds:00}.{centiseconds:00}";
    }

    public void GameOver()
    {
        gameRunning = false;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        if (scoreText != null)
            scoreText.text = $"Temps survécu :\n{minutes:00}:{seconds:00}";

        if (PlayerPrefs.HasKey("ArenaTimer"))
        {
            float bestTime = PlayerPrefs.GetFloat("ArenaTimer");
            Debug.Log($"[GameOver] HasKey=true | bestTime={bestTime} | timer={timer}");

            if (timer > bestTime)
            {
                PlayerPrefs.SetFloat("ArenaTimer", timer);
                PlayerPrefs.Save();
                bestTime = timer;
            }

            int bMin = Mathf.FloorToInt(bestTime / 60f);
            int bSec = Mathf.FloorToInt(bestTime % 60f);
            if (bestTimer != null)
                bestTimer.text = $"Meilleur temps : {bMin:00}:{bSec:00}";
            else
                Debug.LogError("[GameOver] bestTimer est NULL !");
        }
        else
        {
            Debug.Log($"[GameOver] HasKey=false | timer={timer}");

            PlayerPrefs.SetFloat("ArenaTimer", timer);
            PlayerPrefs.Save();

            int bMin = Mathf.FloorToInt(timer / 60f);
            int bSec = Mathf.FloorToInt(timer % 60f);
            if (bestTimer != null)
                bestTimer.text = $"Meilleur temps : {bMin:00}:{bSec:00}";
            else
                Debug.LogError("[GameOver] bestTimer est NULL !");
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LobbyScene");
    }
}