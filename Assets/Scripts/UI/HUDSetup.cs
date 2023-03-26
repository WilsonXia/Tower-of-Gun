using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDSetup : MonoBehaviour
{
    // Data
    [SerializeField]
    GameObject levelInfo;
    LevelManager level;

    // Text Elements
    [SerializeField]
    TextMeshProUGUI progressLabel;
    [SerializeField]
    TextMeshProUGUI waveLabel;

    private void Start()
    {
        level = levelInfo.GetComponent<LevelManager>();
    }
    // Update is called once per frame
    void Update()
    {
        waveLabel.text = string.Format("Wave {0}/{1}", level.WaveCount + 1, level.MaxWave + 1);
        progressLabel.text = string.Format("{0} pts Until Next Wave", level.WaveScoreCapacity - level.WaveScore);
    }
}
