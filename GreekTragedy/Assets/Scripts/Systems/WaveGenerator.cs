using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveGenerator : MonoBehaviour
{
    public static int CurrentWave = 1;
    [SerializeField] int maxWaves;
    [SerializeField] int enemiesPerWave;
    [SerializeField, Tooltip("Every wave this adds to itself and then adds to enemy count, leave 0 for no effect")] 
    int enemiesPerWaveAdd;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] GameObject[] enemies;
    [SerializeField] UnityEvent OnStartedWave;
    [SerializeField] UnityEvent<GameObject> OnEnemySpawned;
    [SerializeField] UnityEvent OnFinishedWave;
    List<GameObject> _currentEnemies = new();
    bool _startedSpawning = false;

    private void Update()
    {
        if (_currentEnemies.Count == 0 && _startedSpawning)
        {
            _startedSpawning = false;
            OnFinishedWave.Invoke();
        }
    }

    private void OnDisable() => StopAllCoroutines();

    public void StartSpawningWaves()
    {
        OnStartedWave.Invoke();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        _currentEnemies.Clear();
        _startedSpawning = true;
        int totalToSpawn = enemiesPerWave + enemiesPerWaveAdd;
        for (int i = 0; i < enemiesPerWave; i++)
        {
            yield return new WaitForSecondsRealtime(timeBetweenSpawns);
            Vector2 newPos = new Vector2(Random.Range(-PlayerMove.ROOM_SIZE.x * .5f, PlayerMove.ROOM_SIZE.x * .5f), Random.Range(-PlayerMove.ROOM_SIZE.y * .5f, PlayerMove.ROOM_SIZE.y * .5f));
            GameObject e = Instantiate(enemies[maxWaves % enemies.Length], newPos, Quaternion.identity);
            _currentEnemies.Add(e);
            OnEnemySpawned?.Invoke(e);
        }
        enemiesPerWaveAdd += enemiesPerWaveAdd;
        CurrentWave++;
        if (CurrentWave < maxWaves)
        {
            yield return new WaitForSecondsRealtime(timeBetweenWaves);
            StartCoroutine(SpawnWave());
        }
    }
}
