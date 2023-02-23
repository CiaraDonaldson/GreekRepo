using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WaveGenerator : MonoBehaviour
{
    public static int CurrentWave = 1;
    [SerializeField] int maxWaves;
    [SerializeField, Tooltip("Every wave has this static amount plus Adds if any")] int enemiesPerWaveStatic;
    [SerializeField, Tooltip("Every wave this adds to itself and then adds to enemy count, leave 0 for no effect")] 
    int enemiesPerWaveAdds;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] GameObject[] enemies;
    [SerializeField] UnityEvent OnStartedWave;
    [SerializeField] UnityEvent<GameObject> OnEnemySpawned;
    [SerializeField] UnityEvent OnClearedCurrentWave;
    [SerializeField] UnityEvent OnFinishedAllWaves;
    readonly List<GameObject> _currentEnemies = new();
    bool _finishedSpawning = false;
    bool hasStarted;

    private void Update()
    {
        if (!hasStarted & _finishedSpawning & _currentEnemies.Count == 0)
        {
            hasStarted = true;
            OnClearedCurrentWave?.Invoke();
        }
    }

    private void OnEnable()
    {
        Enemy.OnEnemyDied += RemoveEnemyFromList;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= RemoveEnemyFromList;
        StopAllCoroutines();
    }

    [ContextMenu(nameof(SpawnWave))]
    public void StartSpawningWaves()
    {
        OnStartedWave.Invoke();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (CurrentWave == maxWaves)
        {
            OnFinishedAllWaves?.Invoke();
            yield return null;
        }
        int totalToSpawn = enemiesPerWaveStatic + enemiesPerWaveAdds;
        for (int i = 0; i < totalToSpawn; i++)
        {
            yield return new WaitForSecondsRealtime(timeBetweenSpawns);
            Vector2 newPos = new Vector2(Random.Range(-PlayerMove.ROOM_SIZE.x * .5f, PlayerMove.ROOM_SIZE.x * .5f), Random.Range(-PlayerMove.ROOM_SIZE.y * .5f, PlayerMove.ROOM_SIZE.y * .5f));
            GameObject e = Instantiate(enemies[maxWaves % enemies.Length], newPos, Quaternion.identity);
            _currentEnemies.Add(e);
            OnEnemySpawned?.Invoke(e);
        }
        enemiesPerWaveAdds += enemiesPerWaveAdds;
        CurrentWave++;
        _finishedSpawning = true;
        hasStarted = false;
    }

    void RemoveEnemyFromList(GameObject obj) => _currentEnemies.Remove(obj);
}
