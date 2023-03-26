using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInterface : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    GameObject player;

    [SerializeField]
    Slider healthBar;
    private void Start()
    {
        healthBar.maxValue = player.GetComponent<ObjectStats>().Health;
    }

    // Update is called once per frame
    void Update()
    {
        // Update Player health
        healthBar.value = player.GetComponent<ObjectStats>().Health;
    }
}
