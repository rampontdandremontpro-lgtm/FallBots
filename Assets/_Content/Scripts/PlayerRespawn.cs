using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;

    private void Start()
    {
        spawnPoint = transform.position;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        CharacterController cc = GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = spawnPoint;
        cc.enabled = true;

        Player playerScript = GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.ResetVelocity();
        }
    }

    // Pour les checkpoints
    public void SetSpawn(Vector3 newSpawn)
    {
        spawnPoint = newSpawn;
    }
}