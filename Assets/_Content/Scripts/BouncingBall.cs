using UnityEngine;

public class BouncingBall : MonoBehaviour
{
    [SerializeField] private float pushForce = 10f;

    public enum BallSize
    { Small, Medium, Large, XL }

    [SerializeField] private BallSize size;

    private void Start()
    {
        switch (size)
        {
            case BallSize.Small:
                pushForce = 5f;
                break;

            case BallSize.Medium:
                pushForce = 15f;
                break;

            case BallSize.Large:
                pushForce = 25f;
                break;

            case BallSize.XL:
                pushForce = 35f;
                break;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            CharacterController cc = collision.GetComponent<CharacterController>();
            if (cc != null)
            {
                Vector3 pushDirection = (collision.transform.position - transform.position).normalized;
                cc.Move(pushDirection * pushForce * Time.deltaTime);
            }
        }
    }
}