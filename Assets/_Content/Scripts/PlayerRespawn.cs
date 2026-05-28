using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 spawnPoint;

    void Start()
    {
        spawnPoint = transform.position;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Death"))
        {
            Respawn();
        }
    }

    void Respawn()
    {
        // Téléportation (le CC doit être désactivé sinon transform.position est ignoré)
        CharacterController cc = GetComponent<CharacterController>();
        cc.enabled = false;
        transform.position = spawnPoint;
        cc.enabled = true;

        // Reset la vélocité via la propriété publique State
        Player playerScript = GetComponent<Player>();
        if (playerScript != null)
        {
            playerScript.State.Velocity = Vector3.zero;
        }
    }

    // Pour les checkpoints
    public void SetSpawn(Vector3 newSpawn)
    {
        spawnPoint = newSpawn;
    }
}