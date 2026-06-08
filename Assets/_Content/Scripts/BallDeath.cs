using UnityEngine;

public class BallDeath : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("death"))
        {
            Destroy(gameObject);
            return;
        }

        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            Vector3 direction = (collision.transform.position - transform.position).normalized;
            player.ApplyKnockback(direction, 10f);
        }
    }
}