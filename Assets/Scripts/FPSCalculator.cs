using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;

public class FPSCalculator : MonoBehaviour
{
    public float updateInterval = 0.5f; //How often should the number update

    float accum = 0.0f;
    int frames = 0;
    float timeleft;
    float fps;

    TextMeshProUGUI fpsText;
    public TextMeshProUGUI playerText;
    public TextMeshProUGUI enemyText;

    public EnemyPlayer enemy;
    public FlyCamera player;

    // Start is called before the first frame update
    void Start()
    {
        timeleft = updateInterval;
        fpsText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        timeleft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        ++frames;

        // Interval ended - update GUI text and start new interval
        if (timeleft <= 0.0)
        {
            // display two fractional digits (f2 format)
            string playerName = PlayerPrefs.GetString("playerName");
            if (playerName ==  null || playerName == "")
            {
                playerName = "Tucusito";
            }
            fps = (accum / frames);
            fpsText.text = "FPS: " + Math.Round(fps, 2);
            if (player != null)
            {

                float life = player.life;
                if (life < 0.01f)
                {
                    enemyText.text = playerName + " a Muerto!";
                }
                else
                    fpsText.text = playerName + ": " + Math.Round(player.life * 10.0f, 2);
            }
            if (enemy != null)
            {
                float life = enemy.life;
                if (life < 0.01f)
                {
                    enemyText.text = "Baba a Muerto!";
                } else
                    enemyText.text = "Baba: " + Math.Round(enemy.life * 10.0f, 2);
            }
            timeleft = updateInterval;
            accum = 0.0f;
            frames = 0;
        }
    }
}
