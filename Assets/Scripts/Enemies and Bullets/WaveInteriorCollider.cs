using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveInteriorCollider : MonoBehaviour
{
    [SerializeField] private PulseTest waveScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            waveScript.interiorCollision = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            waveScript.interiorCollision = false;
            Player playerScript = collision.gameObject.GetComponent<Player>();
            if (waveScript.exteriorCollision && (playerScript.invincibilityTimer <= 0) && !playerScript.playerDashing)
            {
                playerScript.TakeDamage();
            }
        }
    }
}
