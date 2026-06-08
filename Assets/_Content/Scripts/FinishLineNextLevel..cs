using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class FinishLineNextLevel : MonoBehaviour
{
    [Header("UI References")]
    [Tooltip("Le panneau bleu (GameObject 'Screen' dans la hiérarchie Canvas)")]
    public GameObject finishScreen;

    [Tooltip("TextMeshPro affichant le score / timer (enfant de Screen)")]
    public TextMeshProUGUI scoreText;

    [Tooltip("TextMeshPro 'Appuyez sur ESPACE...' (enfant de Screen)")]
    public TextMeshProUGUI promptText;

    [Header("Scene Management")]
    [Tooltip("Nom exact de la scène suivante dans Build Settings")]
    public string nextSceneName = "Level2";

    [Header("Breathing Animation")]
    [Tooltip("Durée d'un demi-cycle respiration (fade in OU fade out), en secondes")]
    [Range(0.3f, 3f)]
    public float breathDuration = 1.2f;

    [Tooltip("Opacité minimale du texte ESPACE (0 = invisible)")]
    [Range(0f, 0.5f)]
    public float minAlpha = 0.1f;

    [Tooltip("Opacité maximale du texte ESPACE (1 = pleinement visible)")]
    [Range(0.5f, 1f)]
    public float maxAlpha = 1f;

    [Header("Screen Fade-in")]
    [Tooltip("Durée d'apparition du panneau bleu, en secondes")]
    [Range(0.1f, 2f)]
    public float screenFadeInDuration = 0.5f;

    // ── état interne ──────────────────────────────────────────────
    private float _elapsedTime = 0f;   // timer qui tourne dès le début

    private bool _levelFinished = false;
    private bool _canProceed = false; // vrai une fois le panneau affiché

    // ─────────────────────────────────────────────────────────────
    private void Start()
    {
        // On s'assure que le panneau est caché au départ
        if (finishScreen != null)
            finishScreen.SetActive(false);

        if (promptText != null)
        {
            Color c = promptText.color;
            c.a = 0f;
            promptText.color = c;
        }
    }

    // ─────────────────────────────────────────────────────────────
    private void Update()
    {
        // Timer en cours de partie
        if (!_levelFinished)
        {
            _elapsedTime += Time.deltaTime;
        }

        // Appui sur ESPACE pour passer au niveau suivant
        if (_canProceed && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            LoadNextScene();
        }
    }

    // ─────────────────────────────────────────────────────────────
    private void OnTriggerEnter(Collider other)
    {
        if (_levelFinished) return;
        if (!other.CompareTag("Player")) return;

        _levelFinished = true;
        ShowFinishUI();
    }

    // ─────────────────────────────────────────────────────────────
    private void ShowFinishUI()
    {
        if (finishScreen == null) return;

        finishScreen.SetActive(true);

        // Affiche le score (temps formaté mm:ss)
        if (scoreText != null)
        {
            int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
            scoreText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }

        // Lance les animations
        StartCoroutine(FadeInScreen());
    }

    // ─────────────────────────────────────────────────────────────
    /// Fait apparaître le panneau en fondu, puis lance la respiration du prompt
    private IEnumerator FadeInScreen()
    {
        // Récupère le CanvasGroup ou crée-en un pour le panneau
        CanvasGroup cg = finishScreen.GetComponent<CanvasGroup>();
        if (cg == null)
            cg = finishScreen.AddComponent<CanvasGroup>();

        cg.alpha = 0f;
        float t = 0f;

        while (t < screenFadeInDuration)
        {
            t += Time.deltaTime;
            cg.alpha = Mathf.Clamp01(t / screenFadeInDuration);
            yield return null;
        }

        cg.alpha = 1f;
        _canProceed = true;

        // Lance la respiration du texte ESPACE
        if (promptText != null)
            StartCoroutine(BreathingText());
    }

    // ─────────────────────────────────────────────────────────────
    /// Animation "respiration" : le texte monte et descend en opacité en boucle
    private IEnumerator BreathingText()
    {
        while (true) // boucle infinie jusqu'à chargement scène
        {
            // Fade IN  (minAlpha → maxAlpha)
            yield return StartCoroutine(FadeText(promptText, minAlpha, maxAlpha, breathDuration));

            // Fade OUT (maxAlpha → minAlpha)
            yield return StartCoroutine(FadeText(promptText, maxAlpha, minAlpha, breathDuration));
        }
    }

    // ─────────────────────────────────────────────────────────────
    private IEnumerator FadeText(TextMeshProUGUI tmp, float from, float to, float duration)
    {
        float t = 0f;
        Color c = tmp.color;

        while (t < duration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(from, to, t / duration);
            tmp.color = c;
            yield return null;
        }

        c.a = to;
        tmp.color = c;
    }

    // ─────────────────────────────────────────────────────────────
    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
            SceneManager.LoadScene(nextSceneName);
        else
            Debug.LogWarning("[LevelFinishTrigger] Aucune scène suivante définie !");
    }
}