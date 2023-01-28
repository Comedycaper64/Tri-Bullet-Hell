using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedLevel1 : MonoBehaviour
{
    private bool[] tasks = new bool[100];
    private bool[] repeatedTasks = new bool[100];
    public BulletSpawner bulletSpawner;

    private void Start()
    {
        Time.fixedDeltaTime = 0.001f;
    }

    private void FixedUpdate()
    {
        for (int i = 0; i < 10; i++)
        {
            if (TimeManager.isTime((float)i, ref repeatedTasks[i * 10]))
            {
                if (i < 5)
                {
                    bulletSpawner.bulletSpawnDataIndex = 1;
                }
                else
                {
                    bulletSpawner.bulletSpawnDataIndex = 0;
                }
                bulletSpawner.SpawnBullets();
            }
        }
        /*
        if (TimeManager.isTime(1f, ref tasks[0]))
        {
            bulletSpawner.bulletSpawnDataIndex = 0;
            bulletSpawner.SpawnBullets();
        }
        if (TimeManager.isTime(2f, ref tasks[1]))
        {
            bulletSpawner.bulletSpawnDataIndex = 0;
            bulletSpawner.SpawnBullets();
        }
        if (TimeManager.isTime(3f, ref tasks[2]))
        {
            bulletSpawner.bulletSpawnDataIndex = 1;
            bulletSpawner.SpawnBullets();
        }
        */
    }


}
