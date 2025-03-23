using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
public class WaveSystem : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject minigunPrefab;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI waveCompletion;
    public TextMeshProUGUI waveCompletion1;
    public GameObject waveCompletionMenu;
    private float spawnRange = 6;
    private int enemyCount;
    public int waveCount = 1;
    public bool canSpawn;
    public bool checkSpawn;
    public bool gameActive;
    public bool startNext;
    public bool gameCompleted;
    public int FinalWave = 14;

    public AudioSource soundPlayer;
    public AudioClip completePlay;
    public AudioClip finalWaveSound;

    // Start is called before the first frame update
    void Start()
    {
        soundPlayer = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

        enemyCount = FindObjectsOfType<EnemyHandler>().Length;
        if (checkSpawn && enemyCount == 0)
        {
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
            waveText.text = "Wave: " + waveCount;
            SpawnEnemyWave(waveCount);

            if (waveCount == 15)
            {
                waveText.text = "Final Wave";
            }
            else
            {
                waveText.text = "Wave: " + waveCount;
            }

            if (waveCount % 5 == 0)
            {
                Instantiate(minigunPrefab, GenerateSpawnPosition(), minigunPrefab.transform.rotation);
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
            Instantiate(minigunPrefab, GenerateSpawnPosition(), minigunPrefab.transform.rotation);

        }
    }

    IEnumerator DelayWave()
    {
        waveCompletion.gameObject.SetActive(true);
        waveCompletion.text = "Wave Completed!";
        yield return new WaitForSeconds(4);
        waveCompletion.gameObject.SetActive(false);
        startNext = true;
        checkSpawn = false;
        StopAllCoroutines();

        if (enemyCount == 0 && waveCount == 15)
        {
            gameCompleted = true;
            GameCompleted();
        }
    }

    IEnumerator RemainingWaveIndicator()
    {
        if (gameCompleted) yield break;

        int remainingWaves = FinalWave - waveCount;


        if (remainingWaves == 1 && waveCount == 14)
        {
            soundPlayer.PlayOneShot(finalWaveSound);
            waveCompletion1.gameObject.SetActive(true);
            waveCompletion1.text = "You have the FINAL wave left!";
        }
        else if (remainingWaves > 1)
        {
            waveCompletion1.gameObject.SetActive(true);
            waveCompletion1.text = $"You now have {remainingWaves} waves left!";
        }

        yield return new WaitForSeconds(2);
        waveCompletion1.gameObject.SetActive(false);
        yield return new WaitForSeconds(1);
        canSpawn = true;
        StopAllCoroutines();
    }

    public void GameCompleted()
    {
        soundPlayer.Stop();
        soundPlayer.PlayOneShot(completePlay, 0.1f);
        Debug.Log("Game Completed!");
        gameCompleted = true;

        waveCompletionMenu.gameObject.SetActive(true);
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
        float SpawnPosX = Random.Range(-spawnRange, spawnRange);
        float SpawnPosZ = Random.Range(-spawnRange, spawnRange);

        Vector3 randomPos = new Vector3(SpawnPosX, 1, SpawnPosZ);

        return randomPos;
    }
}
