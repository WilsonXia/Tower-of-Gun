using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ObjectStats))]
[RequireComponent(typeof(CollisionInfo))]
public abstract class Enemy : MonoBehaviour
{
    // Fields
    ObjectStats stats;
    
    // Time
    [SerializeField]
    protected float timer;
    [SerializeField]
    protected float observeSec;
    [SerializeField]
    protected float waitSec;

    [SerializeField]
    protected float detectRadius;

    protected bool onScreenFlag;

    protected EnemyState currentState;
    protected Player player;

    [SerializeField]
    protected Vector3 position = Vector3.zero;
    [SerializeField]
    protected Vector3 velocity = Vector3.zero;
    [SerializeField]
    protected Vector3 direction;

    // Properties
    public Player Player { get { return player; } set { player = value; } }
    public Vector2 Position { get { return position; } set { position = value; } }
    public bool OnScreen { get { return onScreenFlag; } set { onScreenFlag = value; } }

    void Start()
    {
        stats = GetComponent<ObjectStats>();
        position = transform.position;
        onScreenFlag = false;
        GetComponent<CollisionInfo>().IsHostile = true;
        Setup();
    }

    void Update()
    {
        Timer();
        Movement();
    }

    // Virtual
    #region Virtual
    public virtual void Setup(){}
    public virtual void Movement() {
        transform.position = position;
    }
    public virtual void Timer() {
        timer += Time.deltaTime;
    }
#endregion
    // Behaviors
    #region Behaviors
    protected void HuntPlayer()
    {
        direction = Vector3.Normalize(player.Position - position);
        velocity = direction * stats.Speed * Time.deltaTime;
        position += velocity;
    }

    protected void FlyStraight(float speedModifier = 1f)
    {
        velocity = direction.normalized * stats.Speed * Time.deltaTime * speedModifier;
        position += velocity;
    }
    /// <summary>
    /// Causes the enemy to move according to a Sin wave
    /// </summary>
    /// <param name="freq">the frequency of the graph (1/Period)</param>
    /// <param name="ampl">the absolute maximum value of the graph</param>
    protected void ZigZag(float freq, float ampl)
    {
        direction.y = Mathf.Sin(timer*freq) * ampl;//freq and ampl
        FlyStraight(1.2f);
    }

    protected void StandStill()
    {
        direction = Vector2.zero;
    }
#endregion
    // Helper Methods
    #region Helper Methods
    protected bool DetectPlayer() 
    {
        float sqrDistFromPlayer = Vector3.SqrMagnitude(player.Position - transform.position);
        return sqrDistFromPlayer < Mathf.Pow(detectRadius, 2);
    }
    protected void InvertVelocity()
    {
        // Invert Velocity
        direction = velocity.normalized;
        if (Random.value > 0.5)
        {
            //direction.y *= 0;
            direction.x *= -1;
        }
        else
        {
            //direction.x *= 0;
            direction.y *= -1;
        }
    }
    /// <summary>
    /// Checks if an Enemy has existed long enough to Observe their next action
    /// </summary>
    /// <returns></returns>
    protected bool ExistLongEnough()
    {
        // Is on screen and can observe
        return onScreenFlag && timer > observeSec;
    }
    #endregion
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + direction);
    }
}
public enum EnemyState
{
    Enter,
    Observe,
    Retreat,
    Attack,
    Special
}