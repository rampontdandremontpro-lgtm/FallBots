using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Génère une grille de tuiles hexagonales en niveaux, 
/// comme dans Fall Guys Hex-A-Gone.
/// </summary>
public class HexGridGenerator : MonoBehaviour
{
    [Header("Prefab")]
    [Tooltip("Prefab de la tuile hexagonale (doit avoir le script HexTile)")]
    public GameObject hexTilePrefab;

    [Header("Grille")]
    [Tooltip("Nombre de colonnes de tuiles")]
    public int columns = 9;

    [Tooltip("Nombre de lignes de tuiles")]
    public int rows = 7;

    [Tooltip("Nombre de niveaux (étages) de la map")]
    public int levels = 3;

    [Header("Espacement")]
    [Tooltip("Taille d'une tuile hexagonale")]
    public float tileSize = 1.2f;

    [Tooltip("Espace vertical entre chaque niveau")]
    public float levelSpacing = 4f;

    [Header("Couleurs par niveau")]
    public Color[] levelColors = new Color[]
    {
        new Color(1f, 0.8f, 0.1f),   // Jaune - niveau 1
        new Color(1f, 0.5f, 0.1f),   // Orange - niveau 2
        new Color(0.9f, 0.2f, 0.2f)  // Rouge - niveau 3
    };

    // Stockage de toutes les tuiles générées
    private List<HexTile> allTiles = new List<HexTile>();

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        // Espacement hex : 
        // Horizontal : tileSize * sqrt(3)
        // Vertical   : tileSize * 1.5
        float hexWidth  = tileSize * Mathf.Sqrt(3f);
        float hexHeight = tileSize * 1.5f;

        for (int level = 0; level < levels; level++)
        {
            float yPos = -level * levelSpacing;
            Color tileColor = level < levelColors.Length ? levelColors[level] : Color.white;

            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    // Décalage en X pour les rangées impaires (pattern hexagonal)
                    float offsetX = (row % 2 == 0) ? 0 : hexWidth / 2f;

                    float xPos = col * hexWidth + offsetX;
                    float zPos = row * hexHeight;

                    Vector3 position = new Vector3(xPos, yPos, zPos) + transform.position;

                    GameObject tile = Instantiate(hexTilePrefab, position, Quaternion.identity, transform);
                    tile.name = $"HexTile_L{level}_R{row}_C{col}";

                    // Appliquer la couleur du niveau
                    Renderer rend = tile.GetComponent<Renderer>();
                    if (rend != null)
                    {
                        rend.material.color = tileColor;
                    }

                    HexTile hexTile = tile.GetComponent<HexTile>();
                    if (hexTile != null)
                        allTiles.Add(hexTile);
                }
            }
        }

        Debug.Log($"[HexGridGenerator] Grille générée : {levels} niveaux, {allTiles.Count} tuiles au total.");
    }

    /// <summary>
    /// Réinitialise toutes les tuiles (pour restart de niveau).
    /// </summary>
    public void ResetAllTiles()
    {
        foreach (var tile in allTiles)
        {
            if (tile != null)
                tile.ResetTile();
        }
    }

    /// <summary>
    /// Vérifie si toutes les tuiles d'un niveau sont tombées.
    /// </summary>
    public bool IsLevelCleared(int level)
    {
        foreach (var tile in allTiles)
        {
            if (tile != null && tile.gameObject.name.Contains($"_L{level}_"))
                return false;
        }
        return true;
    }
}
