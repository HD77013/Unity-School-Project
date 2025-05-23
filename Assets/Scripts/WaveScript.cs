using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
public class WaveSystem : MonoBehaviour
{
    public GameObject enemyPrefab;
    private float _spawnRange = 6;
    private int _enemyCount;
    public int waveCount = 1;
    public bool canSpawn;
    public bool checkSpawn;
    public bool gameActive;
    public bool startNext;
    public bool gameCompleted;
    public int finalWave = 14;


    // Start is called before the first frame update
    void Start()
    {
        StartGame();
        gameActive = true;
    }

    // Update is called once per frame
    void Update()
    {

        _enemyCount = FindObjectsOfType<EnemyScript>().Length;
        if (checkSpawn && _enemyCount == 0) {
            StartCoroutine(DelayWave());
        }

        if (startNext)
        {
            StartCoroutine(RemainingWaveIndicator());
            startNext = false;
        }

        if (canSpawn)
        {
            canSpawn = false;
            waveCount++;
            SpawnEnemyWave(waveCount);

            if (waveCount == 15)
            {
            }
            else
            {
            }

            if (waveCount % 5 == 0)
            {
            }

            checkSpawn = true;
        }

    }

    public void StartGame()
    {
        if (gameActive)
        {
            Debug.Log("Game begins!");
            checkSpawn = true;
            canSpawn = false;
            SpawnEnemyWave(waveCount);

        }
    }

    IEnumerator DelayWave()
    {

        yield return new WaitForSeconds(4);
        startNext = true;
        checkSpawn = false;
        StopAllCoroutines();

        if (_enemyCount == 0 && waveCount == 15)
        {
            gameCompleted = true;
            GameCompleted();
        }
    }

    IEnumerator RemainingWaveIndicator()
    {
        if (gameCompleted) yield break;

        int remainingWaves = finalWave - waveCount;


        if (remainingWaves == 1 && waveCount == 14)
        {

        }
        else if (remainingWaves > 1)
        {

        }

        yield return new WaitForSeconds(2);

        yield return new WaitForSeconds(1);
        canSpawn = true;
        StopAllCoroutines();
    }

    public void GameCompleted()
    {

        Debug.Log("Game Completed!");
        gameCompleted = true;

    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        float SpawnPosX = Random.Range(-_spawnRange, _spawnRange);
        float SpawnPosZ = Random.Range(-_spawnRange, _spawnRange);

        Vector3 randomPos = new Vector3(SpawnPosX, 1.27f, SpawnPosZ);

        return randomPos;
    }
}
