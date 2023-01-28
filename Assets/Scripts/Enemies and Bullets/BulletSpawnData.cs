using UnityEngine;

[CreateAssetMenu(fileName = "BulletData", menuName = "ScriptableObjects/BulletSpawnData", order = 1)]
public class BulletSpawnData : ScriptableObject
{
    [Header("Bullet")]
    public GameObject bulletResource;
    public Sprite bulletSprite;
    public float minRotation;
    public float maxRotation;
    public int numberOfBullets;
    public bool isNotParent;
    public float minCooldown;
    public float maxCooldown;
    public bool isRandom;
    public float bulletSpeed;
    public float bulletSize;
    public float bulletLifetime;
    public Vector2 bulletVelocity;
    [Header("Wave")]
    public bool solidWave;
    public GameObject waveResource;
    public float waveLifetime;
    public float waveGrowthRate;
    
}
