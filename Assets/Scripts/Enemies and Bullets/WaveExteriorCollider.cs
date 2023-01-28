using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveExteriorCollider : MonoBehaviour
{
    [SerializeField] private PulseTest waveScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            waveScript.exteriorCollision = true;
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (!waveScript.interiorCollision && (playerScript.invincibilityTimer <= 0) && !playerScript.playerDashing)
            {
                playerScript.TakeDamage();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            waveScript.exteriorCollision = false;
        }
    }
}
