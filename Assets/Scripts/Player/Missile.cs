using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public float lifetime;
    public int damage;
    public float speed;

    [SerializeField] private AudioClip explosionSFX;
    private MusicPlayer musicPlayer;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifetime);
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += (transform.TransformDirection(Vector3.up) * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position, musicPlayer.currentVolume);
            collision.gameObject.GetComponent<Enemy>().TakeDamage(damage);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Obstacle"))
        {
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position, musicPlayer.currentVolume);
            Destroy(gameObject);
        }
    }
}
