using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRemoveCell : MonoBehaviour
{
    [Header("Références")]
    public Transform[] etages; // Glisse tes 5 étages ici

    [Header("Timing")]
    public float interval = 3f; // Toutes les 3 secondes

    void Start()
    {
        StartCoroutine(RemoveCellsRoutine());
    }

    private IEnumerator RemoveCellsRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            foreach (Transform etage in etages)
            {
                RemoveRandomCell(etage);
            }
        }
    }

    private void RemoveRandomCell(Transform etage)
    {
        // Récupère toutes les cellules encore actives dans cet étage
        List<HexTile> cellules = new List<HexTile>();

        foreach (Transform child in etage)
        {
            HexTile tile = child.GetComponent<HexTile>();
            if (tile != null && !tile.isTriggered)
                cellules.Add(tile);
        }

        if (cellules.Count == 0) return; // Plus de cellules sur cet étage

        // Choisit une cellule aléatoire et la fait tomber
        int randomIndex = Random.Range(0, cellules.Count);
        cellules[randomIndex].TriggerFall();

        Debug.Log($"Cellule supprimée sur : {etage.name}");
    }
}
