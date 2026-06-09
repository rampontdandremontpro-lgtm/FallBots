using System.Collections;
using UnityEngine;

public class BoutonActivateur : MonoBehaviour
{
    [Header("Plaques à activer")]
    public PlaqueCachee[] plaques;

    [Header("Timing")]
    public float delaiEntreChaque = 0.3f;

    [Header("Visuel bouton")]
    public Color couleurActive = Color.green;

    public Color couleurNormale = Color.red;

    private bool dejaDeclanche = false;
    private Renderer boutonRenderer;

    private void Start()
    {
        boutonRenderer = GetComponent<Renderer>();
        if (boutonRenderer != null)
            boutonRenderer.material.color = couleurNormale;
    }

    private void Update()
    {
        if (dejaDeclanche) return;

        Collider[] colliders = Physics.OverlapBox(
            transform.position + Vector3.up * 0.1f,
            new Vector3(transform.localScale.x * 0.4f, 0.2f, transform.localScale.z * 0.4f)
        );

        // DEBUG
        if (colliders.Length > 0)
        {
            foreach (Collider col in colliders)
                Debug.Log("Detecte : " + col.gameObject.name + " Tag : " + col.gameObject.tag);
        }

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("Player"))
            {
                dejaDeclanche = true;
                StartCoroutine(ActiverPlaques());
                break;
            }
        }
    }

    private IEnumerator ActiverPlaques()
    {
        if (boutonRenderer != null)
            boutonRenderer.material.color = couleurActive;

        foreach (PlaqueCachee plaque in plaques)
        {
            if (plaque != null)
            {
                plaque.Activer();
                yield return new WaitForSeconds(delaiEntreChaque);
            }
        }
    }
}