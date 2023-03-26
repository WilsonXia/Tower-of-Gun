using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class ObjectStats : MonoBehaviour
{
    [SerializeField]
    float maxHealth;
    float health;
    [SerializeField]
    float damage;
    [SerializeField]
    float speed;
    [SerializeField]
    int score;

    [SerializeField]
    bool ignoreHealth;
    bool isDead;

    // Properties
    public float Health { get { return health; } set { health = value; } }
    public float MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public float Damage { get { return damage; } set { damage = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public int Score { get { return score; } set { score = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    // Start is called before the first frame update
    void Start()
    {
        // defaults
        health = maxHealth;
        isDead = false;
    }

    private void Update()
    {
        if (!ignoreHealth)
        {
            if (health <= 0)
            {
                isDead = true;
            }
            else if (health > maxHealth)
            {
                health = maxHealth;
            }
        }
    }

    public void TakeDamage(float damage) 
    {
        if(damage > health)
        {
            health = 0;
        }
        else
        {
            health -= damage;
        }
    }
}
