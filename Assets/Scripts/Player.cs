using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(ObjectStats))]
public class Player : MonoBehaviour
{
    LevelManager level;
    ObjectStats stats;
    [SerializeField]
    Gun playerGun;
    [SerializeField]
    float immunitySec;
    [SerializeField]
    bool isImmune;
    float iTimer;
    Vector3 mousePos;

    // Properties
    public float Health { get { return stats.Health; } set { stats.Health = value; } }
    public float MaxHealth { get { return stats.MaxHealth; } set { stats.MaxHealth = value; } }
    public Vector3 Position { get { return transform.position; } }
    public bool IsImmune { get { return isImmune; } set { isImmune = value; } }
    public LevelManager LevelManger { get { return level; } set { level = value; } }
    void Start()
    {
        stats = GetComponent<ObjectStats>();
        playerGun.LevelManager = level;
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        if (isImmune)
        {
            GetComponent<SpriteRenderer>().color = Color.grey;
            iTimer += Time.deltaTime;
            if(iTimer > immunitySec)
            {
                GetComponent<SpriteRenderer>().color = Color.white;
                isImmune = false;
                iTimer = 0;
            }
        }
        TrackMouse();
        //Rotate();
    }

    void TrackMouse()
    {
        // Calculate Mouse Position
        mousePos = Mouse.current.position.ReadValue();
        // Put it into World Space
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        // Reset the z
        mousePos.z = 0;
        // Rotate towards mouse
        transform.rotation = Quaternion.Euler(0, 0,
            Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x)
            * Mathf.Rad2Deg - 90);
    }

    //void Rotate()
    //{
    //    // Makes sure the angle goes the full 360 degrees
    //    //bool mouseOver = mousePos.x < 0;
    //    //if (mouseOver)
    //    //{

    //    //}
    //    Vector3 desiredAngle = mousePos - transform.position;
    //    float angle = Mathf.Atan2(desiredAngle.y, desiredAngle.x);
        
    //    transform.Rotate(0,0, angle * speed * Time.deltaTime); 
    //    //Debug.Log("Mouse Over? " + mouseOver);
    //    //Debug.Log("Mouse: " + mousePos.normalized + "\t Up: " + transform.up);
    //}

    public void SetUp()
    {
        stats.Health = stats.MaxHealth;
        playerGun.IsHostile = false;
        iTimer = 0;
        isImmune = false;
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, mousePos.normalized);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.up);
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, mousePos - transform.position);
    }
}
