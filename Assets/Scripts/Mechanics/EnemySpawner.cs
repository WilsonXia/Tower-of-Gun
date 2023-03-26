using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpawnPattern
{
    PatternA, PatternB, Testing
}
public class EnemySpawner : MonoBehaviour
{
    // Fields
    public LevelManager level;

    [SerializeField]
    SpawnPattern spawnPattern;
    [SerializeField]
    float spawnRadius;
    [SerializeField]
    float spawnMargin;
    [SerializeField]
    float spawnCooldown;
    [SerializeField]
    float timer = 0;

    public SpawnPattern CurrentPattern { get { return spawnPattern; } set { spawnPattern = value; } }

    void Start()
    {
        timer = spawnCooldown;
    }

    private void Update()
    {
        SpawnEnemies();
    }

    public void SpawnEnemies()
    {
        if (timer > spawnCooldown)
        {
            switch (spawnPattern)
            {
                case SpawnPattern.PatternA:
                    PatternA();
                    break;
                case SpawnPattern.PatternB:
                    PatternB();
                    break;
                default:
                    SpawnEnemy(0);
                    break;
            }
            timer = 0;
        }
        timer += Time.deltaTime;
    }
    #region Spawn Patterns
    void PatternA()
    {
        if (Random.value < 0.3)
        {
            SpawnCross(0);
        }
        else
        {
            int numEnemies = Random.Range(2,4);
            int index = 0;
            if(numEnemies < 3)
                index = 1;
            if(Random.value < 0.5)
            {
                SpawnRow(index, numEnemies);
            }
            else
            {
                SpawnCol(index, numEnemies);
            }
        }
    }
    void PatternB()
    {
        SpawnEnemy();
    }
    #endregion

    #region Spawning Enemies
    void SpawnEnemy()
    {
        SpawnEnemy(Random.Range(0,level.EnemyPrefabs.Count));
    }
    void SpawnEnemy(int index)
    {
        SpawnEnemy(index, GetRandomCoordinate());
    }
    void SpawnEnemy(int index, Vector2 position)
    {
        GameObject newEnemy = Instantiate(level.EnemyPrefabs[index], position, Quaternion.identity,
            level.transform);
        Enemy e = newEnemy.GetComponent<Enemy>();
        e.Player = level.Player;
        level.Enemies.Add(newEnemy);
        level.GameObjects.Add(newEnemy);
        if (e is EnemyB) 
        {
            e.GetComponent<Gun>().LevelManager = level;
        }
    }
    void SpawnRow(int index, int numEnemies, Vector2 center)
    {
        float totalLength = (numEnemies - 1) * spawnMargin;
        float length = totalLength / (numEnemies - 1);
        center.x -= totalLength / 2;
        for (int i = 0; i < numEnemies; i++)
        {
            SpawnEnemy(index, center);
            center.x += length;
        }
    }
    void SpawnRow(int index, int numEnemies)
    {
        if(Random.value > 0.5)
        {
            SpawnRow(index, numEnemies, GetRandomCoordinate(0));
        }
        else
        {
            SpawnRow(index, numEnemies, GetRandomCoordinate(2));
        }
    }
    void SpawnCol(int index, int numEnemies, Vector2 center)
    {
        float totalLength = (numEnemies - 1) * spawnMargin;
        float length = (numEnemies - 1) * spawnMargin / (numEnemies - 1);
        center.y -= totalLength / 2;
        for (int i = 0; i < numEnemies; i++)
        {
            SpawnEnemy(index, center);
            center.y += length;
        }
    }
    void SpawnCol(int index, int numEnemies)
    {
        if (Random.value > 0.5)
        {
            SpawnCol(index, numEnemies, GetRandomCoordinate(1));
        }
        else
        {
            SpawnCol(index, numEnemies, GetRandomCoordinate(3));
        }
    }
    void SpawnCross(int index)
    {
        Vector2 pos = GetRandomCoordinate();
        SpawnRow(index, 3, pos);
        SpawnCol(index, 3, pos);
        // Remove one enemy in the middle
        level.Enemies[level.Enemies.Count - 2].GetComponent<ObjectStats>().IsDead = true;
    }
    #endregion

    #region Random Coordinates
    Vector2 GetRandomCoordinate(int area)
    {
        Vector2 coordinate = Vector2.zero;
        switch (area)
        {
            // Relates in a clock-wise motion, starting with Top
            case 0:
                coordinate = GetCoordinateTop();
                break;
            case 1:
                coordinate = GetCoordinateRight();
                break;
            case 2:
                coordinate = GetCoordinateBottom();
                break;
            default:
                coordinate = GetCoordinateLeft();
                break;
        }
        return coordinate;
    }
    Vector2 GetRandomCoordinate()
    {
        return GetRandomCoordinate((int)(Random.value * 3));
    }

    Vector2 GetCoordinateTop()
    {
        Vector2 coordinate = Vector2.zero;
        // Spawns from the top or bottom
        coordinate.y = level.Bounds.y + Random.Range(0, spawnRadius);
        coordinate.x = Random.Range(-level.Bounds.x, level.Bounds.x);
        return coordinate;
    }
    Vector2 GetCoordinateBottom()
    {
        Vector2 coordinate = GetCoordinateTop();
        coordinate.y *= -1;
        return coordinate;
    }
    Vector2 GetCoordinateRight()
    {
        Vector2 coordinate = Vector2.zero;
        // Spawns from the sides
        coordinate.x = level.Bounds.x + Random.Range(0, spawnRadius);
        coordinate.y = Random.Range(-level.Bounds.y, level.Bounds.y);
        return coordinate;
    }
    Vector2 GetCoordinateLeft()
    {
        Vector2 coordinate = GetCoordinateRight();
        coordinate.x *= -1;
        return coordinate;
    }
    #endregion

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireCube(new Vector3(xBound + spawnRadius / 2, 0, 0), new Vector3(spawnRadius, yBound * 2));
    //    Gizmos.DrawWireCube(new Vector3(-(xBound + spawnRadius / 2), 0, 0), new Vector3(spawnRadius, yBound * 2));
    //    // Bounds of the top and bottom
    //    Gizmos.DrawWireCube(new Vector3(0, -(yBound + spawnRadius / 2), 0), new Vector3(xBound * 2, spawnRadius));
    //    Gizmos.DrawWireCube(new Vector3(0, (yBound + spawnRadius / 2), 0), new Vector3(xBound * 2, spawnRadius));
    //}
}
