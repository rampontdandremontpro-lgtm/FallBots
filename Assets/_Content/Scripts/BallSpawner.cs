using UnityEngine;

public class BallSpawner : MonoBehaviour
{
    public GameObject[] ballPrefabs;
    public float spawnInterval = 1f;
    public float ballLifetime = 10f;
    public float spawnRadius = 5f;

    [Header("Rebond aléatoire")]
    public float minBounciness = 0.2f;

    public float maxBounciness = 1f;
    public float fallSpeed = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnBall), 0f, spawnInterval);
    }

    private void SpawnBall()
    {
        // Choisit un prefab au hasard dans le tableau
        GameObject prefab = ballPrefabs[Random.Range(0, ballPrefabs.Length)];

        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPos = transform.position + new Vector3(randomOffset.x, 0f, randomOffset.y);

        GameObject ball = Instantiate(prefab, spawnPos, Quaternion.identity);

        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
            rb.linearVelocity = Vector3.down * fallSpeed;

        // Rebond aléatoire
        float bounciness = Random.Range(minBounciness, maxBounciness);
        PhysicsMaterial bounceMat = new PhysicsMaterial("BallMat");
        bounceMat.bounciness = bounciness;
        bounceMat.bounceCombine = PhysicsMaterialCombine.Maximum;

        Collider col = ball.GetComponent<Collider>();
        if (col != null)
            col.material = bounceMat;

        ball.AddComponent<BallDeath>();
        Destroy(ball, ballLifetime);
    }
}