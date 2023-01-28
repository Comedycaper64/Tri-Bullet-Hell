using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Vector2 velocity;
    [SerializeField] private float speed;
    [SerializeField] private float rotation;
    [SerializeField] private float bulletLifetime;
    [SerializeField] private float lifeTimer;

    // Start is called before the first frame update
    void Start()
    {
        lifeTimer = bulletLifetime;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(velocity * speed * Time.deltaTime);
        lifeTimer -= Time.deltaTime;
        if (lifeTimer <= 0)
        {
            SetBulletInactive();
        }
    }

    public void SetBulletInactive()
    {
        gameObject.SetActive(false);
    }

    public void ResetTimer()
    {
        lifeTimer = bulletLifetime;
    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }

    public void SetLifetime(float lifeTime)
    {
        bulletLifetime = lifeTime;
        this.lifeTimer = lifeTime;
    }

    public void SetRotation(float rotation)
    {
        this.rotation = rotation;
        transform.rotation = Quaternion.Euler(0, 0, rotation);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Obstacle"))
        {
            //audio.PlayOneShot(explosionSFX);
            SetBulletInactive();
        }
    }
}
