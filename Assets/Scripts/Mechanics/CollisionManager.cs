using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    public LevelManager level;

    // Overall Check
    public void CheckCollisions()
    {
        CheckBulletEnemy();
        CheckBulletBullet();
        CheckPlayerBullet();
        CheckPlayerEnemy();
    }

    // Check Bullet Collisions
    void CheckBulletEnemy() 
    {
        for(int i = 0; i < level.FBullets.Count; i++)
        {
            GameObject bullet = level.FBullets[i];
            for(int j = 0; j < level.Enemies.Count; j++)
            {
                GameObject mob = level.Enemies[j];
                if (AABBCollision(mob, bullet))
                {
                    // Responses
                    mob.GetComponent<ObjectStats>().
                        TakeDamage(
                        CombineDamage(
                            bullet.GetComponent<ObjectStats>().Damage)); // Take Damage
                    bullet.GetComponent<Projectile>().Bounces -= 1; // Bullet takes "damage"
                    if(mob.GetComponent<ObjectStats>().Health <= 0) // If you defeat an enemy, raise score
                    {
                        level.RaiseScore(mob.GetComponent<ObjectStats>().Score);
                    }
                }
            }
        }
    }
    void CheckBulletBullet() 
    {
        for (int i = 0; i < level.FBullets.Count; i++)
        {
            GameObject bullet = level.FBullets[i];
            for (int j = 0; j < level.HBullets.Count; j++)
            {
                GameObject bad = level.HBullets[j];
                if (AABBCollision(bad, bullet))
                {
                    bad.GetComponent<Projectile>().Bounces -= 1; // bullet takes "damage"
                    bullet.GetComponent<Projectile>().Bounces -= 1; 
                }
            }
        }
    }

    // Check Player Collisions
    void CheckPlayerBullet()
    {
        for (int i = 0; i < level.HBullets.Count; i++)
        {
            GameObject bullet = level.HBullets[i];
            if (AABBCollision(level.Player.gameObject, bullet))
            {
                PlayerTakeDamage(bullet.GetComponent<ObjectStats>().Damage);
                bullet.GetComponent<Projectile>().Bounces -= 1; // bullet takes "damage"
            }
        }
    }
    void CheckPlayerEnemy()
    {
        for(int i = 0; i < level.Enemies.Count; i++)
        {
            GameObject mob = level.Enemies[i];
            if (mob.GetComponent<CollisionInfo>().IsHostile)
            {
                if(AABBCollision(level.Player.gameObject, mob))
                {
                    PlayerTakeDamage(mob.GetComponent<ObjectStats>().Damage);
                    mob.GetComponent<ObjectStats>().IsDead = true; // Kill enemy
                }
            }
        }
    }

    // Collision Responses
    void PlayerTakeDamage(float damage)
    {
        Player p = level.Player;
        if (!p.IsImmune)
        {
            p.GetComponent<ObjectStats>().TakeDamage(damage);
            p.IsImmune = true;
        }
    }

    float CombineDamage(float damage) 
    {
        return level.Player.GetComponent<ObjectStats>().Damage + damage;
    }

    // AABB Collision
    bool AABBCollision(GameObject a, GameObject b) 
    {
        return AABBCollision(a.GetComponent<CollisionInfo>(),b.GetComponent<CollisionInfo>());
    }
    bool AABBCollision(CollisionInfo a, CollisionInfo b) 
    {
        bool verdict = a.MinX < b.MaxX && a.MaxX > b.MinX
            && a.MinY < b.MaxY && a.MaxY > b.MinY;
        return verdict;
    }
    // Circle Collision
    //bool CircleCollision(GameObject a, GameObject b) 
    //{
    //    float sqrDistance = Vector3.SqrMagnitude(a.transform.position - b.transform.position);
    //    return CircleCollision(a.GetComponent<CollisionInfo>(), b.GetComponent<CollisionInfo>(), sqrDistance);
    //}

    //bool CircleCollision(CollisionInfo a, CollisionInfo b, float dist)
    //{
    //    return dist < Mathf.Pow(a.Radius + b.Radius, 2);
    //}
}
