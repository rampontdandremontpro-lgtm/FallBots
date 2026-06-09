using System.Collections;
using UnityEngine;

public class PlaqueCachee : MonoBehaviour
{
    [Header("Position")]
    public float hauteurCachee = -2f;

    public float hauteurVisible = 0f;

    [Header("Mouvement")]
    public float vitesseMontee = 2f;

    [Header("Chute")]
    public float delaiChute = 2f;

    private bool estActivee = false;
    private bool monteeTerminee = false;
    private bool isTriggered = false;
    private Rigidbody rb;
    private Collider col;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();

        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        Vector3 pos = transform.position;
        pos.y = hauteurCachee;
        transform.position = pos;
    }

    private void Update()
    {
        if (!monteeTerminee || isTriggered) return;

        Bounds bounds = col.bounds;

        Collider[] colliders = Physics.OverlapBox(
            bounds.center + Vector3.up * 0.1f,
            new Vector3(bounds.extents.x * 0.8f, 0.2f, bounds.extents.z * 0.8f)
        );

        foreach (Collider c in colliders)
        {
            if (c.CompareTag("Player"))
            {
                isTriggered = true;
                StartCoroutine(ChuteSequence());
                break;
            }
        }
    }

    public void Activer()
    {
        if (!estActivee)
        {
            estActivee = true;
            StartCoroutine(Monter());
        }
    }

    private IEnumerator Monter()
    {
        while (!monteeTerminee)
        {
            float newY = Mathf.Lerp(transform.position.y, hauteurVisible, vitesseMontee * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);

            if (Mathf.Abs(transform.position.y - hauteurVisible) < 0.01f)
            {
                transform.position = new Vector3(transform.position.x, hauteurVisible, transform.position.z);
                monteeTerminee = true;
            }

            yield return null;
        }
    }

    private IEnumerator ChuteSequence()
    {
        yield return new WaitForSeconds(delaiChute);

        // ✅ Activer la physique pour la chute
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse); // pousse vers le bas immédiatement
        }

        // ✅ Désactiver le collider APRÈS un délai pour que le joueur tombe avec la plaque
        yield return new WaitForSeconds(0.5f);

        if (col != null)
            col.enabled = false;

        Destroy(gameObject, 3f);
    }
}