using System.Collections;
using UnityEngine;

/// <summary>
/// Met sur chaque plaque cachée.
/// Elle démarre sous le sol et monte quand activée.
/// </summary>
public class PlaqueCachee : MonoBehaviour
{
    [Header("Position")]
    public float hauteurCachee = -2f;   // Position Y cachée sous le sol
    public float hauteurVisible = 0f;   // Position Y finale visible

    [Header("Mouvement")]
    public float vitesseMontee = 2f;    // Vitesse du lerp

    private bool estActivee = false;
    private bool monteeTerminee = false;

    void Start()
    {
        // Démarre cachée sous le sol
        Vector3 pos = transform.position;
        pos.y = hauteurCachee;
        transform.position = pos;
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

            // Arret quand proche de la destination
            if (Mathf.Abs(transform.position.y - hauteurVisible) < 0.01f)
            {
                transform.position = new Vector3(transform.position.x, hauteurVisible, transform.position.z);
                monteeTerminee = true;
            }

            yield return null;
        }
    }
}
