using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyC : Enemy
{
    [SerializeField]
    protected float ampl;
    [SerializeField]
    protected float freq;
    //Overrides
    public override void Setup()
    {
        // 5hp, 3spd
        ampl = 1;
        freq = 15;
        detectRadius = 5f;
        currentState = EnemyState.Enter;
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
                FlyStraight(0.1f);
                break;
            case EnemyState.Retreat:
                ZigZag(freq, ampl);
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
                // On screen and entered long enough
                if(ExistLongEnough())
                {
                    currentState = EnemyState.Observe;
                    timer = 0;
                }
                break;
            case EnemyState.Observe:
                // Waited
                if(timer > waitSec)
                {
                    currentState = EnemyState.Enter;
                    timer = 0;
                }
                break;
        }
        if (DetectPlayer() && currentState != EnemyState.Retreat)
        {
            currentState = EnemyState.Retreat;
            timer = 0;
        }
    }
}
