using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    GameObject bulletPrefab;
    [SerializeField]
    bool isHostile;
    [SerializeField]
    int bulletCapacity = 5;
    [SerializeField]
    float fireCooldown;

    float timer;
    List<GameObject> projectiles;
    LevelManager level;
    
    public bool IsHostile { get { return isHostile; } set { isHostile = value; } }
    public LevelManager LevelManager{ get { return level; } set { level = value; } }
    void Start()
    {
        projectiles = new List<GameObject>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        for(int i = 0; i < projectiles.Count; i++)
        {
            if(projectiles[i] == null)
            {
                projectiles.RemoveAt(i);
                i--;
            }
        }
    }

    public void Fire()
    {
        Fire(transform.up);
    }
    public void Fire(Vector3 dir)
    {
        // Instantiate a bullet into the list
        if (projectiles.Count < bulletCapacity && timer > fireCooldown)
        {
            // Reset Timer
            timer = 0;
            // Create the Bullet
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            newBullet.GetComponent<Projectile>().Direction = dir;
            newBullet.GetComponent<CollisionInfo>().IsHostile = isHostile;
            // Add it to Lists
            projectiles.Add(newBullet);
            level.GameObjects.Add(newBullet);
            if (isHostile)
            {
                level.HBullets.Add(newBullet);
            }
            else
            {
                level.FBullets.Add(newBullet);
            }
        }
    }
}
