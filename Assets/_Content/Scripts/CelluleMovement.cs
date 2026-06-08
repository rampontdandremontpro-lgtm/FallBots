using UnityEngine;

public class CelluleMovement : MonoBehaviour
{
    [Header("Mouvement")]
    public float amplitude = 1f;

    public float speed = 1f;

    private float[] startY;
    private float[] randomOffsets;

    private void Start()
    {
        int count = transform.childCount;
        startY = new float[count];
        randomOffsets = new float[count];

        for (int i = 0; i < count; i++)
        {
            startY[i] = transform.GetChild(i).position.y;
            // Offset vraiment aléatoire pour chaque cellule
            randomOffsets[i] = Random.Range(0f, Mathf.PI * 2f);
        }
    }

    private void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform cellule = transform.GetChild(i);
            float newY = startY[i] + Mathf.Sin(Time.time * speed + randomOffsets[i]) * amplitude;
            cellule.position = new Vector3(cellule.position.x, newY, cellule.position.z);
        }
    }
}