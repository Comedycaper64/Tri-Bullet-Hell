using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool dying;
    [SerializeField] private int health;
    EnemyWaveConfig waveConfig;
    private int waypointIndex = 0;
    [SerializeField] private List<Transform> waypoints;

    [Header("Audio")]
    [SerializeField] private AudioClip takeDamage;
    private MusicPlayer musicPlayer;
    // Start is called before the first frame update
    void Start()
    {
        dying = false;
        waypoints = waveConfig.GetWaypoints();
        transform.position = waypoints[waypointIndex].transform.position;
        musicPlayer = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicPlayer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (waypointIndex <= waypoints.Count - 1)
        {
            var targetPosition = waypoints[waypointIndex].transform.position;
            var movementThisFrame = waveConfig.GetMoveSpeed() * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, movementThisFrame);
            transform.up = Vector3.Lerp(transform.up, (targetPosition - transform.position), 3 * Time.deltaTime);
            if (Mathf.Abs((transform.position - targetPosition).magnitude) < 0.05f)
            {
                waypointIndex++;
            }
        }
        else
        {
            waypointIndex = 1;
        }
    }

    public void SetWaveConfig(EnemyWaveConfig waveConfig)
    {
        this.waveConfig = waveConfig;
    }

    public void TakeDamage(int damage)
    {
        health = health - damage;
        AudioSource.PlayClipAtPoint(takeDamage, transform.position, musicPlayer.currentVolume);
        if ((health < 1) && !dying)
        {
            dying = true;
            Die();
        }
    }

    private void Die()
    {
        EnemySpawner spawner = GameObject.FindGameObjectWithTag("Spawner").GetComponent<EnemySpawner>();
        spawner.DecreaseAliveEnemies();
        Destroy(gameObject);
    }
}
