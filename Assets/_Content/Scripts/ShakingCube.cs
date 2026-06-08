using UnityEngine;
using System.Collections;

public class ShakingPlatform : MonoBehaviour
{
    public float shakeDuration = 0.5f;
    public float shakeAmount = 0.1f;
    public float waitBetweenShakes = 2f;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
        StartCoroutine(ShakeLoop());
    }

    private IEnumerator ShakeLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(waitBetweenShakes);

            float elapsed = 0f;

            while (elapsed < shakeDuration)
            {
                float x = Random.Range(-shakeAmount, shakeAmount);
                float z = Random.Range(-shakeAmount, shakeAmount);

                transform.position = startPosition + new Vector3(x, 0, z);

                elapsed += Time.deltaTime;
                yield return null;
            }

            transform.position = startPosition;
        }
    }
}
