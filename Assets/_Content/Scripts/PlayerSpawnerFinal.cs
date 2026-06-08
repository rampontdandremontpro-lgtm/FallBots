using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [Header("Références")]
    public Transform player;        // Ton joueur
    public Transform etage5;        // L'Empty Etage5

    void Start()
    {
        SpawnOnRandomCell();
    }

    void SpawnOnRandomCell()
    {
        // Récupère tous les enfants de Etage5 (tes cellules)
        int childCount = etage5.childCount;

        if (childCount == 0)
        {
            Debug.LogWarning("Etage5 est vide !");
            return;
        }

        // Choisit une cellule aléatoire
        int randomIndex = Random.Range(0, childCount);
        Transform randomCell = etage5.GetChild(randomIndex);

        // Spawn le joueur au-dessus de la cellule
        Vector3 spawnPos = randomCell.position;
        spawnPos.y += 1.5f; // Au-dessus de la cellule

        player.position = spawnPos;

        Debug.Log($"Joueur spawné sur : {randomCell.name}");
    }
}
