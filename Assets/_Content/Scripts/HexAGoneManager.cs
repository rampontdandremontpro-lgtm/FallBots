using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gère le déroulement du niveau Hex-A-Gone en mode solo.
/// - Détecte si le joueur est tombé (mort)
/// - Affiche le timer
/// - Gère le Game Over / Victoire (survie)
/// </summary>
public class HexAGoneManager : MonoBehaviour
{
    [Header("Références")]
    public Transform player;
    public HexGridGenerator gridGenerator;

    [Header("Limites")]
    [Tooltip("Hauteur Y en dessous de laquelle le joueur est considéré mort")]
    public float deathY = -15f;

    [Header("Timer de survie")]
    [Tooltip("Durée du niveau en secondes (0 = pas de limite)")]
    public float survivalTime = 0f;
    private float timer = 0f;
    private bool gameRunning = false;

    [Header("UI")]
    public TextMeshProUGUI timerText;
    public GameObject gameOverPanel;
    public GameObject victoryPanel;
    public TextMeshProUGUI finalTimeText;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!gameRunning) return;

        // Mise à jour du timer
        timer += Time.deltaTime;
        UpdateTimerUI();

        // Vérifier si le joueur est mort (tombé)
        if (player != null && player.position.y < deathY)
        {
            GameOver();
        }

        // Vérifier condition de victoire (survie temporisée)
        if (survivalTime > 0f && timer >= survivalTime)
        {
            Victory();
        }
    }

    void StartGame()
    {
        timer = 0f;
        gameRunning = true;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        Debug.Log("[HexAGoneManager] Jeu démarré !");
    }

    void UpdateTimerUI()
    {
        if (timerText == null) return;

        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        int centiseconds = Mathf.FloorToInt((timer * 100f) % 100f);

        timerText.text = $"{minutes:00}:{seconds:00}.{centiseconds:00}";
    }

    void GameOver()
    {
        if (!gameRunning) return;
        gameRunning = false;

        Debug.Log($"[HexAGoneManager] GAME OVER ! Survie : {timer:F2}s");

        if (finalTimeText != null)
            finalTimeText.text = $"Temps survécu : {timer:F1}s";

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        // Arrêter le joueur
        if (player != null)
        {
            Rigidbody rb = player.GetComponent<Rigidbody>();
            if (rb != null) rb.isKinematic = true;
        }
    }

    void Victory()
    {
        if (!gameRunning) return;
        gameRunning = false;

        Debug.Log("[HexAGoneManager] VICTOIRE !");

        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    // Appelé par le bouton "Rejouer" dans l'UI
    public void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex
        );
    }
}
