using System.Collections;
using UnityEngine;

public class SpaceBackground : MonoBehaviour
{
    [Header("Etoiles filantes")]
    public int nombreEtoilesFilantes = 5;
    public float intervalleEtoile = 3f;
    public float vitesseEtoile = 20f;
    public float tailleEtoile = 0.1f;
    public Color couleurEtoile = Color.white;

    [Header("Zone de spawn")]
    public float zoneSize = 30f;

    void Start()
    {
        StartCoroutine(SpawnEtoilesFilantes());
    }

    private IEnumerator SpawnEtoilesFilantes()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(intervalleEtoile * 0.5f, intervalleEtoile));

            for (int i = 0; i < nombreEtoilesFilantes; i++)
            {
                StartCoroutine(LancerEtoileFilante());
                yield return new WaitForSeconds(0.1f);
            }
        }
    }

    private IEnumerator LancerEtoileFilante()
    {
        // Position de départ aléatoire en haut
        Vector3 startPos = new Vector3(
            Random.Range(-zoneSize, zoneSize),
            Random.Range(5f, 20f),
            Random.Range(-zoneSize, zoneSize)
        );

        // Direction aléatoire vers le bas
        Vector3 direction = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, -0.3f),
            Random.Range(-1f, 1f)
        ).normalized;

        // Créer la traînée avec LineRenderer
        GameObject etoile = new GameObject("EtoileFilante");
        LineRenderer lr = etoile.AddComponent<LineRenderer>();

        lr.startWidth = tailleEtoile;
        lr.endWidth = 0f;
        lr.positionCount = 2;
        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = couleurEtoile;
        lr.endColor = new Color(couleurEtoile.r, couleurEtoile.g, couleurEtoile.b, 0f);

        float distance = 0f;
        float maxDistance = Random.Range(10f, 20f);

        while (distance < maxDistance)
        {
            Vector3 currentPos = startPos + direction * distance;
            Vector3 trailStart = startPos + direction * Mathf.Max(0, distance - 3f);

            lr.SetPosition(0, currentPos);
            lr.SetPosition(1, trailStart);

            distance += vitesseEtoile * Time.deltaTime;
            yield return null;
        }

        Destroy(etoile);
    }
}
