using System.Collections;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [Header("Timing")]
    public float fallDelay = 2f;

    public float destroyDelay = 3f;

    [Header("Visual Feedback")]
    public Color warningColor = new Color(1f, 0.4f, 0f);

    private Material tileMaterial;
    private Color originalColor;

    [HideInInspector] public bool isTriggered = false;
    private Rigidbody rb;
    private Collider tileCollider;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        tileCollider = GetComponent<Collider>();

        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            tileMaterial = rend.material;
            originalColor = tileMaterial.color;
        }
    }

    private void Update()
    {
        if (!isTriggered)
        {
            Collider[] colliders = Physics.OverlapBox(
                transform.position + Vector3.up * 0.1f,
                new Vector3(transform.localScale.x * 0.4f, 0.2f, transform.localScale.z * 0.4f)
            );

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Player"))
                {
                    TriggerFall();
                    break;
                }
            }
        }
    }

    // Public pour être appelé par AutoRemoveCell
    public void TriggerFall()
    {
        if (isTriggered) return;
        isTriggered = true;
        StartCoroutine(FallSequence());
    }

    private IEnumerator FallSequence()
    {
        // Phase 1 : Clignotement orange
        float elapsed = 0f;
        while (elapsed < fallDelay)
        {
            if (tileMaterial != null)
            {
                float t = Mathf.PingPong(elapsed * 8f, 1f);
                tileMaterial.color = Color.Lerp(originalColor, warningColor, t);
            }
            elapsed += Time.deltaTime;
            yield return null;
        }

        // Phase 2 : La cellule tombe
        if (tileMaterial != null)
            tileMaterial.color = warningColor;

        rb.isKinematic = false;
        rb.useGravity = true;

        yield return new WaitForSeconds(0.2f);
        if (tileCollider != null)
            tileCollider.enabled = false;

        // Phase 3 : Destruction
        Destroy(gameObject, destroyDelay);
    }

    public void ResetTile()
    {
        isTriggered = false;
        rb.isKinematic = true;
        rb.useGravity = false;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (tileCollider != null)
            tileCollider.enabled = true;

        if (tileMaterial != null)
            tileMaterial.color = originalColor;
    }
}