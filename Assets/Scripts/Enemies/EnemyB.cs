using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Gun))]
public class EnemyB : Enemy
{
    [SerializeField]
    Gun gun;
    [SerializeField]
    float fireSpeed;
    [SerializeField]
    int ammo;
    int fireCount;
    // Overrides
    public override void Setup()
    {
        // Stats
        //6hp 3spd
        waitSec = 1;

        // State
        currentState = EnemyState.Enter;

        // Gun
        gun.IsHostile = true;
        base.Setup();
    }
    public override void Movement()
    {
        // Switch Case
        switch (currentState)
        {
            case EnemyState.Enter:
                HuntPlayer();
                break;
            case EnemyState.Observe:
                StandStill();
                break;
            case EnemyState.Attack:
                StandStill();
                break;
            case EnemyState.Retreat:
                FlyStraight();
                break;
        }
        base.Movement();
    }
    public override void Timer()
    {
        // Checks for SM Changes
        base.Timer();
        switch (currentState)
        {
            case EnemyState.Enter:
                if (ExistLongEnough() || DetectPlayer())
                {
                    currentState = EnemyState.Observe;
                    observeSec = 99; // Makes it impossible to exist long enough
                    timer = 0;
                }
                break;
            case EnemyState.Observe:
                if(timer >= waitSec)
                {
                    currentState = EnemyState.Attack;
                    fireCount = 0; // Clears count to allow firing
                    timer = fireSpeed; // Shoots immediately
                }
                break;
            case EnemyState.Attack:
                if(timer >= fireSpeed)
                {
                    if (fireCount >= ammo) // We don't have ammo left
                    {
                        currentState = EnemyState.Retreat;
                        InvertVelocity();
                    }
                    else
                    {
                        gun.Fire(Vector3.Normalize(player.Position - transform.position));
                        fireCount++;
                        timer = 0;
                    }
                }
                break;
        }
    }
}
