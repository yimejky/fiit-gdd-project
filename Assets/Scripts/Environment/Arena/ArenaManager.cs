using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaManager : MonoBehaviour, IInteractableObject
{
    public int enemiesToFinish = 10;
    public int maxSpawnedEnemiesAtTime = 3;
    public List<Transform> spawnPoints;
    public List<GameObject> enemies;

    private bool wasActivated = false;
    private bool isActive = false;
    private int killedEnemies;
    private int remainingEnemiesSpawn;
    private GameObject gates;
    private Transform player;
    private Transform enemiesParent;
    private FlashMessage flashMessage;
    private Text arenaText;

    void Start()
    {
        player = GameObject.Find("Hero").transform;
        gates = transform.Find("Gates").gameObject;
        enemiesParent = transform.Find("Enemies");
        flashMessage = GameObject.Find("CameraCanvas").transform.Find("FlashMessage").GetComponent<FlashMessage>();
        arenaText = transform.Find("ArenaCameraCanvas").Find("Panel").Find("ArenaText").GetComponent<Text>();
        arenaText.enabled = false;

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

    private void CloseArena()
    {
        gates.SetActive(true);
    }

    private void OpenArena()
    {
        gates.SetActive(false);
    }

    private void StartArena()
    {
        if (!wasActivated)
        {
            killedEnemies = 0;
            remainingEnemiesSpawn = enemiesToFinish;

            Debug.Log("Start Arena");
            wasActivated = true;
            CloseArena();
            isActive = true;
            arenaText.enabled = true;

            arenaText.text = $"Remaining kills: {enemiesToFinish}";
            flashMessage.ShowMessage("Arena Started!");
        }
    }

    private void FinishArena()
    {
        Debug.Log($"Finish Arena");
        OpenArena();
        isActive = false;
        arenaText.enabled = false;

        flashMessage.ShowMessage("Arena Finished!");
    }

    public void Interact()
    {
        StartArena();
    }

    private void SpawnEnemy()
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
        enemy.endAttackingStateDistance = 100f;
        enemy.SetAttackingState(player.gameObject);

        spawnEnemy.GetComponent<HealthController>().HealthUpdateEvent.AddListener(OnEnemiesHealthUpdate);
        enemies.Add(spawnEnemy);
    }

    private void OnEnemiesHealthUpdate(GameObject enemy, GameObject attacker, int actualHealth)
    {
        if (actualHealth <= 0)
        {
            enemies.Remove(enemy);
            enemy.GetComponent<HealthController>().HealthUpdateEvent.RemoveAllListeners();
            killedEnemies += 1;

            arenaText.text = $"Remaining kills: {enemiesToFinish - killedEnemies}";
        }
    }
}
