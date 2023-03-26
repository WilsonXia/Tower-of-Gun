using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CollisionInfo))]
[RequireComponent(typeof(ObjectStats))]
public class Projectile : MonoBehaviour
{
    // Properties
    ObjectStats stats;
    [SerializeField]
    float lifeTime = 10f;
    [SerializeField]
    int bounceCount = 0;

    [SerializeField]
    Camera main;
    float xBound;
    float yBound;

    [SerializeField]
    Vector2 direction = Vector2.up;
    Vector2 position = Vector2.zero;
    [SerializeField]
    Vector2 velocity = Vector2.zero;

    public float Lifetime { get { return lifeTime; } set { lifeTime = value; } }
    public int Bounces { get { return bounceCount; } set { bounceCount = value; } }
    public Vector2 Direction { get { return direction; } set { direction = value; } }


    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<ObjectStats>();
        main = Camera.main;
        yBound = main.orthographicSize;
        xBound = yBound * main.aspect;
        position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // If life time is up, destroy this object
        if (lifeTime <= 0 || OutOfBounds() || bounceCount < 0)
        {
            stats.IsDead = true;
        }
        Movement();
        lifeTime -= Time.deltaTime;
    }

    void Movement()
    {
        // Update so that the projectile moves in the direction
        // it is fired in.
        velocity = direction * stats.Speed * Time.deltaTime;
        position += velocity;
        transform.position = position;
    }

    bool OutOfBounds()
    {
        if (position.x > xBound || position.x < -xBound ||
            position.y > yBound || position.y < -yBound)
        {
            return true;
        }
        return false;
    }
}
