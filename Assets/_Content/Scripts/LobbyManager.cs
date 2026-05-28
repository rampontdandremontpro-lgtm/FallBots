using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : MonoBehaviour
{
    [Header("Scene")]
    [SerializeField] private string firstLevelSceneName = "PlayerScene";

    [Header("Personnage")]
    [SerializeField] private GameObject[] characterPrefabs;
    [SerializeField] private Transform characterSpawnPoint;
    [SerializeField] private float rotationSpeed = 40f;

    private GameObject currentCharacter;
    private int currentIndex;

    private void Start()
    {
        currentIndex = PlayerPrefs.GetInt("SelectedCharacter", 0);
        ShowCharacter(currentIndex);
    }

    private void Update()
    {
        if (currentCharacter != null)
        {
            currentCharacter.transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
        }
    }

    public void PlayGame()
    {
        PlayerPrefs.SetInt("SelectedCharacter", currentIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(firstLevelSceneName);
    }

    public void NextCharacter()
    {
        currentIndex++;

        if (currentIndex >= characterPrefabs.Length)
            currentIndex = 0;

        ShowCharacter(currentIndex);
    }

    public void PreviousCharacter()
    {
        currentIndex--;

        if (currentIndex < 0)
            currentIndex = characterPrefabs.Length - 1;

        ShowCharacter(currentIndex);
    }

    private void ShowCharacter(int index)
    {
        if (characterPrefabs.Length == 0)
        {
            Debug.LogWarning("Aucun personnage dans Character Prefabs.");
            return;
        }

        if (currentCharacter != null)
            Destroy(currentCharacter);

        currentCharacter = Instantiate(
            characterPrefabs[index],
            characterSpawnPoint.position,
            characterSpawnPoint.rotation
        );

        Animator animator = currentCharacter.GetComponentInChildren<Animator>();

        if (animator != null)
        {
            animator.Play("idle");
        }
    }
}
