using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;
using System.IO;

#endif

/// <summary>
/// Attache ce script sur n'importe quel GameObject dans la scène,
/// clique sur le bouton "Générer Damier" dans l'Inspector,
/// puis retire le script.
/// </summary>
public class CheckerboardMaterial : MonoBehaviour
{
    [Header("Paramètres du damier")]
    [Tooltip("Nombre de cases sur chaque axe (ex: 8 = damier 8x8)")]
    public int tilesCount = 8;

    [Tooltip("Résolution de la texture en pixels (256 ou 512 recommandé)")]
    public int textureSize = 256;

    [Tooltip("Le material à modifier (glisse M_LineWin ici)")]
    public Material targetMaterial;

    [Tooltip("Chemin de sauvegarde dans Assets (ex: Assets/_Content/Textures)")]
    public string savePath = "Assets/_Content/Textures";

#if UNITY_EDITOR

    [ContextMenu("Générer Damier")]
    public void GenerateCheckerboard()
    {
        if (targetMaterial == null)
        {
            Debug.LogError("Assigne d'abord un material dans le champ 'Target Material' !");
            return;
        }

        Texture2D tex = CreateCheckerTexture();

        // Sauvegarde la texture en PNG dans le projet
        if (!Directory.Exists(savePath))
            Directory.CreateDirectory(savePath);

        string filePath = savePath + "/T_Checkerboard.png";
        File.WriteAllBytes(filePath, tex.EncodeToPNG());
        AssetDatabase.Refresh();

        // Reimporte avec les bons réglages
        TextureImporter importer = AssetImporter.GetAtPath(filePath) as TextureImporter;
        if (importer != null)
        {
            importer.wrapMode = TextureWrapMode.Repeat;
            importer.filterMode = FilterMode.Point; // bords nets, pixel-art
            importer.mipmapEnabled = true;
            importer.SaveAndReimport();
        }

        // Applique au material
        Texture2D savedTex = AssetDatabase.LoadAssetAtPath<Texture2D>(filePath);
        targetMaterial.mainTexture = savedTex;
        EditorUtility.SetDirty(targetMaterial);
        AssetDatabase.SaveAssets();

        Debug.Log($"[Checkerboard] Texture sauvegardée : {filePath} et appliquée à {targetMaterial.name}");
    }

    private Texture2D CreateCheckerTexture()
    {
        Texture2D tex = new Texture2D(textureSize, textureSize, TextureFormat.RGB24, true);

        int cellSize = textureSize / tilesCount;

        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                int cx = x / cellSize;
                int cy = y / cellSize;
                bool isWhite = (cx + cy) % 2 == 0;
                tex.SetPixel(x, y, isWhite ? Color.white : Color.black);
            }
        }

        tex.Apply();
        return tex;
    }

#endif
}