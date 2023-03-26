using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyA : Enemy
{
    // Overrides
    [SerializeField]
    float speedModifier;
    public override void Setup()
    {
        // Stats
        //5hp 5spd
        observeSec = 0.5f;
        waitSec = 0.3f;

        // State
        currentState = EnemyState.Observe;
        base.Setup();
    }

    public override void Movement()
    {
        switch (currentState)
        {
            case EnemyState.Enter:
                HuntPlayer();
                break;
            case EnemyState.Observe:
                StandStill();
                break;
            case EnemyState.Retreat:
                FlyStraight(speedModifier);
                break;
        }
        base.Movement();
    }

    public override void Timer()
    {
        base.Timer();
        switch (currentState)
        {
            case EnemyState.Enter:
                if (ExistLongEnough())
                {
                    currentState = EnemyState.Observe;
                    timer = 0;
                }
                break;
            case EnemyState.Observe:
                if(timer >= waitSec)
                {
                    currentState = EnemyState.Enter;
                    timer = 0;
                }
                break;
        }
        CheckIfCloseEnough(); // Moves to retreat
    }

    void CheckIfCloseEnough()
    {
        if (DetectPlayer() && currentState != EnemyState.Retreat)
        {
            currentState = EnemyState.Retreat;
            InvertVelocity();
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(Position, velocity);
    }
}
