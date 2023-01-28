using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EnemySpawner : MonoBehaviour
{
    private Player playerScript;
    private Text enemyCounter;
    private bool allWavesSpawned;
    [SerializeField] private GameObject levelEndScreen;
    [SerializeField] private GameObject playerDeadScreen;
    private int aliveEnemies;
    [SerializeField] private int startingWave;
    [SerializeField] private List<EnemyWaveConfig> waveConfigs;

    private void Awake()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemyCounter = GameObject.FindGameObjectWithTag("Enemy Counter").GetComponent<Text>();
        allWavesSpawned = false;
        aliveEnemies = 0;
        startingWave = 0;
    }

    private void Start()
    {
        StartCoroutine(SpawnAllWaves(waveConfigs));
    }

    private void Update()
    {
        if (playerScript.isDead && !playerDeadScreen.activeInHierarchy)
        {
            playerDeadScreen.SetActive(true);
        }

        if (playerScript.isDead && Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    public void DecreaseAliveEnemies()
    {
        aliveEnemies--;
        enemyCounter.text = aliveEnemies.ToString();
        if (aliveEnemies < 1 && allWavesSpawned)
        {
            StartCoroutine(EndLevel());
        }
    }

    private IEnumerator EndLevel()
    {
        levelEndScreen.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public IEnumerator SpawnAllWaves(List<EnemyWaveConfig> waveConfigs)
    {
        for (int waveIndex = startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];

            yield return StartCoroutine(SpawnAllEnemiesInWave(currentWave));
            if (waveIndex == (waveConfigs.Count - 1))
            {
                allWavesSpawned = true;
            }
            yield return new WaitForSeconds(waveConfigs[waveIndex].GetTimeUntilNextWave());
        }
    }

    public IEnumerator SpawnAllEnemiesInWave(EnemyWaveConfig waveConfig)
    {
        for (int enemyCount = 0; enemyCount < waveConfig.GetNumberOfEnemies(); enemyCount++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWaypoints()[0].transform.position, Quaternion.identity);
            aliveEnemies++;
            enemyCounter.text = aliveEnemies.ToString();
            newEnemy.GetComponent<Enemy>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBetweenSpawns());
        }
    }
}
