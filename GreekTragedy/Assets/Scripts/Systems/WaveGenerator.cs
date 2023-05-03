using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class WaveGenerator : MonoBehaviour
{
    public static int CurrentWave = 0;
    [SerializeField] PlayerMove playerMove;
    [SerializeField] TMP_Text currentWaveText;
    [SerializeField] int maxWaves;
    [SerializeField, Tooltip("Every wave has this static amount plus Adds if any")] int enemiesPerWaveStatic;
    [SerializeField, Tooltip("Every wave this adds to itself and then adds to enemy count, leave 0 for no effect")]
    int enemiesPerWaveAdds;
    [SerializeField] float timeBetweenSpawns;
    [SerializeField] GameObject[] enemies;
    [SerializeField] Transform enemiesParent;
    [SerializeField] UnityEvent OnStartedWaves;
    [SerializeField] UnityEvent<GameObject> OnEnemySpawned;
    [SerializeField] UnityEvent OnClearedCurrentWave;
    [SerializeField] UnityEvent OnLastWave;
    [SerializeField] UnityEvent OnFinishedAllWaves;
    readonly List<GameObject> _currentEnemies = new();
    bool _finishedSpawning = false;
    bool hasStarted;

    private void OnEnable()
    {
        CurrentWave = 0;
        currentWaveText.text = $"Waves Left: {Mathf.Abs(maxWaves - CurrentWave - 1)}";
        Enemy.OnEnemyDied += RemoveEnemyFromList;
    }

    private void OnDisable()
    {
        Enemy.OnEnemyDied -= RemoveEnemyFromList;
        StopAllCoroutines();
    }

    private void Update()
    {
        if (!hasStarted & _finishedSpawning & _currentEnemies.Count == 0)
        {
            hasStarted = true;
            OnClearedCurrentWave?.Invoke();
        }
    }

    [ContextMenu(nameof(SpawnWave))]
    public void StartSpawningWaves()
    {
        OnStartedWaves.Invoke();
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        if (CurrentWave == maxWaves && !PlayerHealth.IsDead)
        {
            OnFinishedAllWaves?.Invoke();
            yield break;
        }
        else if (CurrentWave == maxWaves - 1)
            OnLastWave?.Invoke();
        currentWaveText.text = $"Waves Left: {Mathf.Abs(maxWaves - CurrentWave - 1)}";
        int totalToSpawn = enemiesPerWaveStatic + enemiesPerWaveAdds;
        for (int i = 0; i < totalToSpawn; i++)
        {
            yield return new WaitForSecondsRealtime(timeBetweenSpawns);
            Vector2 newPos = new(Random.Range(-playerMove.roomSize.x * .5f, playerMove.roomSize.x * .5f), Random.Range(-playerMove.roomSize.y * .5f, playerMove.roomSize.y * .5f));
            int GOIndex = Mathf.Clamp(CurrentWave, 0, enemies.Length - 1);
            GameObject e = Instantiate(enemies[GOIndex], newPos, Quaternion.identity, enemiesParent);
            _currentEnemies.Add(e);
            OnEnemySpawned?.Invoke(e);
        }
        enemiesPerWaveAdds += enemiesPerWaveAdds;
        CurrentWave++;
        _finishedSpawning = true;
        hasStarted = false;
    }

    void RemoveEnemyFromList(GameObject obj) => _currentEnemies.Remove(obj);

    public void RemoveAllCurrentEnemies()
    {
        foreach (var e in _currentEnemies)
            Destroy(e);
    }

    public void StopSpawningWave() => StopAllCoroutines();
}
