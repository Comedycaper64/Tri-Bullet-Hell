using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    //private BulletManager bulletManager;

    public BulletSpawnData[] spawnDatas;
    public int bulletSpawnDataIndex = 0;

    private float timer;
    private float[] rotations;
    public bool isSequenceRandom;
    public bool spawningAutomatically;

    [Header("Audio")]
    [SerializeField] private AudioClip fireBullet;
    private MusicPlayer musicPlayer;

    BulletSpawnData GetSpawnData()
    {
        return spawnDatas[bulletSpawnDataIndex];
    }

    // Start is called before the first frame update
    void Start()
    {
        //bulletManager = GameObject.FindGameObjectWithTag("BulletManager").GetComponent<BulletManager>();
        timer = UnityEngine.Random.Range(GetSpawnData().minCooldown, GetSpawnData().maxCooldown);
        rotations = new float[GetSpawnData().numberOfBullets];
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>();
        if (!GetSpawnData().isRandom && !GetSpawnData().solidWave)
        {
            DistributedRotations();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (spawningAutomatically)
        {
            if (timer <= 0)
            {
                SpawnBullets();
                timer = UnityEngine.Random.Range(GetSpawnData().minCooldown, GetSpawnData().maxCooldown);
                if (isSequenceRandom)
                {
                    bulletSpawnDataIndex = UnityEngine.Random.Range(0, spawnDatas.Length);
                }
                else
                {
                    bulletSpawnDataIndex++;
                    if (bulletSpawnDataIndex >= spawnDatas.Length) bulletSpawnDataIndex = 0;
                }
            }
            timer -= Time.deltaTime;
        }
    }
    public float[] RandomRotations()
    {
        for (int i = 0; i < GetSpawnData().numberOfBullets; i++)
        {
            rotations[i] = UnityEngine.Random.Range(GetSpawnData().minRotation, GetSpawnData().maxRotation);
        }
        return rotations;
    }

    public float[] DistributedRotations()
    {
        for (int i = 0; i < GetSpawnData().numberOfBullets; i++)
        {
            var fraction = (float)i / (float)(GetSpawnData().numberOfBullets - 1);
            var difference = GetSpawnData().maxRotation - GetSpawnData().minRotation;
            var fractionOfDifference = fraction * difference;
            rotations[i] = fractionOfDifference + GetSpawnData().minRotation;
        }
        //foreach (var rotation in rotations) Debug.Log(rotation);
        return rotations;
    }
    public GameObject[] SpawnBullets()
    {
        if (!GetSpawnData().solidWave)
        {
            rotations = new float[GetSpawnData().numberOfBullets];
            if (GetSpawnData().isRandom)
            {
                RandomRotations();
            }
            else
            {
                DistributedRotations();
            }
            GameObject[] spawnedBullets = new GameObject[GetSpawnData().numberOfBullets];
            AudioSource.PlayClipAtPoint(fireBullet, transform.position, musicPlayer.currentVolume);
            for (int i = 0; i < GetSpawnData().numberOfBullets; i++)
            {
                spawnedBullets[i] = BulletManager.GetBulletFromPool();
                if (spawnedBullets[i] == null)
                {
                    spawnedBullets[i] = Instantiate(GetSpawnData().bulletResource, transform.position, transform.rotation);
                    BulletManager.ParentBulletToManager(spawnedBullets[i]);
                    BulletManager.bullets.Add(spawnedBullets[i]);
                }
                else
                {
                    spawnedBullets[i].transform.position = transform.position;
                }
                Bullet bullet = spawnedBullets[i].GetComponent<Bullet>();
                bullet.transform.localScale = new Vector3(1, 1, 1);
                bullet.SetRotation(rotations[i]);
                bullet.SetSpeed(GetSpawnData().bulletSpeed);
                bullet.SetVelocity(GetSpawnData().bulletVelocity);
                bullet.SetLifetime(GetSpawnData().bulletLifetime);
                bullet.transform.localScale *= GetSpawnData().bulletSize;
                if (GetSpawnData().bulletSprite)
                {
                    bullet.GetComponent<SpriteRenderer>().sprite = GetSpawnData().bulletSprite;
                }
                if (GetSpawnData().isNotParent) spawnedBullets[i].transform.SetParent(null);
            }
            return spawnedBullets;
        }
        else
        {
            GameObject[] spawnedWave = new GameObject[1];
            spawnedWave[0] = Instantiate(GetSpawnData().waveResource, transform.position, transform.rotation);
            PulseTest wave = spawnedWave[0].GetComponent<PulseTest>();
            wave.SetLifetime(GetSpawnData().waveLifetime);
            wave.SetGrowthRate(GetSpawnData().waveGrowthRate);
            return spawnedWave;
        }
    }
}
