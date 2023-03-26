using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
[RequireComponent(typeof(CollisionManager))]
[RequireComponent(typeof(EnemySpawner))]
public class LevelManager : MonoBehaviour
{
    #region Fields
    // Managers
    CollisionManager collManager;
    EnemySpawner enmySpawner;
    // Prefabs
    [SerializeField]
    GameObject playerPrefab;
    [SerializeField]
    List<GameObject> enemyPrefabs;
    // Waves
    int maxWave;
    int waveCount;
    [SerializeField]
    int waveScoreCapacity;
    int waveScore;
    // Spawn Pattern List
    [SerializeField]
    List<SpawnPattern> spawnPatterns;
    // Spawn Information
    [SerializeField]
    Vector3 playerPosition;

    // Player
    GameObject player;
    // Lists
    List<GameObject> enemies;
    List<GameObject> fBullets;
    List<GameObject> hBullets;
    List<GameObject> gameObjects;
    
    // Pause
    public bool pauseGame;
    // Bounds
    float xBound, yBound;
    #endregion

    // Properties
    public Player Player { get { return player.GetComponent<Player>(); } }
    public List<GameObject> GameObjects { get { return gameObjects; } }
    public List<GameObject> Enemies { get { return enemies; } set { enemies = value; } }
    public List<GameObject> FBullets { get { return fBullets; } set { fBullets = value; } }
    public List<GameObject> HBullets { get { return hBullets; } set { hBullets = value; } }
    public List<GameObject> EnemyPrefabs { get { return enemyPrefabs; } set { enemyPrefabs = value; } }
    public Vector2 Bounds { get { return new Vector2(xBound, yBound); } }
    public int WaveCount { get { return waveCount; } }
    public int MaxWave { get { return maxWave; } }
    public int WaveScore { get { return waveScore; } }
    public int WaveScoreCapacity { get { return waveScoreCapacity; } }
    void Start()
    {
        // Spawning
        player = Instantiate(playerPrefab, playerPosition, Quaternion.identity);
        Player.LevelManger = this;
        // Lists
        gameObjects = new List<GameObject>();
        enemies = new List<GameObject>();
        fBullets = new List<GameObject>();
        hBullets = new List<GameObject>();
        // Defaults
        maxWave = spawnPatterns.Count -1;
        waveCount = 0;
        pauseGame = false;
        // Bounds
        yBound = Camera.main.orthographicSize;
        xBound = yBound * Camera.main.aspect;
        // Connecting
        collManager = GetComponent<CollisionManager>();
        collManager.level = this;
        enmySpawner = GetComponent<EnemySpawner>();
        enmySpawner.level = this;
    }

    // Update is called once per frame
    void Update()
    { 
        // Clean up
        CleanUp(gameObjects);
        CleanUp(enemies);
        CleanUp(fBullets);
        CleanUp(hBullets);
        // Checks
        CheckEnemies();
        collManager.CheckCollisions();
        // Destroy Objects
        CheckDeadObjects();
    }
    public void RaiseScore(int score)
    {
        waveScore += score;
        if(waveScore > waveScoreCapacity && waveCount < maxWave)
        {
            waveCount++;
            waveScoreCapacity += (int) Mathf.Ceil(waveScoreCapacity * 1.5f);
            UpdateSpawnPattern();
        }
    }
    void CheckEnemies()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            // Checks if they are on screen or not
            Enemy enemy = enemies[i].GetComponent<Enemy>();
            CollisionInfo enemySize = enemy.GetComponent<CollisionInfo>();
            if (!enemy.OnScreen)
            {
                if (enemySize.MaxX < xBound && enemySize.MinX > -xBound
                    && enemySize.MaxY < yBound && enemySize.MinY > -yBound)
                {
                    enemy.OnScreen = true;
                }
            }
            else
            {
                if (enemySize.MaxX > xBound || enemySize.MinX < -xBound
                    || enemySize.MaxY > yBound || enemySize.MinY < -yBound)
                {
                    enemy.GetComponent<ObjectStats>().IsDead = true;
                }
            }
        }
    }
    void CheckDeadObjects() 
    {
        for(int i = 0; i < gameObjects.Count; i++)
        {
            ObjectStats currentObj = gameObjects[i].GetComponent<ObjectStats>();
            if (currentObj.IsDead)
            {
                Destroy(currentObj.gameObject);
            }
        }
    }

    void UpdateSpawnPattern()
    {
        // Updates spawn pattern based on wave count
        enmySpawner.CurrentPattern = spawnPatterns[waveCount];
    }
    void CleanUp(List<GameObject> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            if (list[i] == null)
            {
                list.RemoveAt(i);
                i--;
            }
        }
    }
}
