using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour, IInteractableObject
{
    public bool wasActivated = false;
    public bool isActive = false;
    public int enemiesToFinish = 2;
    public int maxSpawnedEnemiesAtTime = 1;
    public float elapsedTime = 0;
    public GameObject gates;
    public Transform player;
    public Transform enemiesParent;
    public List<GameObject> enemies;
    public List<Transform> spawnPoints;

    private int killedEnemies;
    private int remainingEnemiesSpawn;

    void Start()
    {
        player = GameObject.Find("Hero").transform;
        gates = transform.Find("Gates").gameObject;
        enemiesParent = transform.Find("Enemies");

        foreach (Transform spawnPoint in transform.Find("SpawnPoints"))
        {
            spawnPoints.Add(spawnPoint);
        }

        OpenArena();
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive) {
            elapsedTime += Time.deltaTime;

            if (enemies.Count < maxSpawnedEnemiesAtTime && remainingEnemiesSpawn > 0)
            {
                remainingEnemiesSpawn -= 1;
                SpawnEnemy();
            }

            if (killedEnemies >= enemiesToFinish && isActive)
            {
                FinishArena();
            }
        }
    }

    void CloseArena()
    {
        gates.SetActive(true);
    }

    void OpenArena()
    {
        gates.SetActive(false);
    }

    void StartArena()
    {
        if (!wasActivated)
        {
            killedEnemies = 0;
            remainingEnemiesSpawn = enemiesToFinish;

            Debug.Log("Start arena!");
            wasActivated = true;
            CloseArena();
            isActive = true;
        }
    }

    void SpawnEnemy()
    {
        int index = Random.Range(0, spawnPoints.Count);
        Transform spawnPoint = spawnPoints[index];
        // Debug.Log($"spawn enemy: {index}, {spawnPoints[index]}");

        string prefabName = "MeleeEnemy";
        Object origin = Resources.Load("Prefabs/Enemies/" + prefabName);
        Vector3 randomOffset = new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
        Vector3 spawnPointPos = spawnPoint.position + randomOffset;

        GameObject spawnEnemy = Instantiate(origin, spawnPointPos, Quaternion.identity, enemiesParent) as GameObject;
        Enemy enemy = spawnEnemy.GetComponent<Enemy>();
        enemy.startAttackingStateDistance = 50f;
        enemy.SetAttackingState(player.gameObject);

        spawnEnemy.GetComponent<HealthController>().HealthUpdateEvent.AddListener(OnEnemiesHealthUpdate);
        enemies.Add(spawnEnemy);
    }

    void OnEnemiesHealthUpdate(GameObject enemy, GameObject attacker, int actualHealth)
    {
        if (actualHealth < 0)
        {
            enemies.Remove(enemy);
            enemy.GetComponent<HealthController>().HealthUpdateEvent.RemoveAllListeners();
            killedEnemies += 1;
        }
    }

    void FinishArena()
    {
        Debug.Log($"Finish arena!");
        OpenArena();
        isActive = false;
    }

    public void Interact()
    {
        StartArena();
    }
}
