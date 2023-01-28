using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public static List<GameObject> bullets;
    private static GameObject bulletManager;

    // Start is called before the first frame update
    void Start()
    {
        bullets = new List<GameObject>();
        bulletManager = gameObject;
    }

    public static void ParentBulletToManager(GameObject bullet)
    {
        bullet.transform.SetParent(bulletManager.transform);
    }

    public static GameObject GetBulletFromPool()
    {
        for (int i = 0; i < bullets.Count; i++)
        {
            if (!bullets[i].activeInHierarchy)
            {
                bullets[i].GetComponent<Bullet>().ResetTimer();
                bullets[i].SetActive(true);
                return bullets[i];
            }
        }
        return null;
    }
}
