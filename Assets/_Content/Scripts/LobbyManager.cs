using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LobbyManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string firstLevelSceneName = "PlateformerScene";

    [Header("Personnage déjà présent dans la scène")]
    [SerializeField] private GameObject currentCharacter;
    [SerializeField] private float rotationSpeed = 40f;

    [Header("UI")]
    [SerializeField] private TMP_Text skinNameText;
    [SerializeField] private TMP_Text skinRarityText;

    private PlayerCustom playerCustom;

    private void Start()
    {
        if (currentCharacter == null)
        {
            Debug.LogWarning("Aucun BotPivot ou Bot assigné dans le LobbyManager.");
            return;
        }

        playerCustom = currentCharacter.GetComponentInChildren<PlayerCustom>();

        if (playerCustom == null)
        {
            Debug.LogWarning("Aucun PlayerCustom trouvé sur le Bot.");
            return;
        }

        UpdateSkinName();
    }

    private void Update()
    {
        if (currentCharacter != null)
        {
            currentCharacter.transform.Rotate(
                Vector3.up,
                rotationSpeed * Time.deltaTime,
                Space.World
            );
        }
    }

    public void PlayGame()
    {
        PlayerPrefs.Save();
        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void RandomSkin()
    {
        if (playerCustom == null)
        {
            Debug.LogWarning("Aucun PlayerCustom trouvé sur le Bot.");
            return;
        }

        playerCustom.Randomize();
        UpdateSkinName();
    }

    private void UpdateSkinName()
    {
        if (playerCustom == null)
            return;

        if (skinNameText != null)
            skinNameText.text = playerCustom.GetSkinName();

        if (skinRarityText != null)
        {
            skinRarityText.text = playerCustom.GetSkinRarity();
            skinRarityText.color = playerCustom.GetSkinRarityColor();
        }
    }
}
