using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuEngine : MonoBehaviour
{

    public AudioSource clickAudio;
    public GameObject principal;
    public GameObject creditos;
    public GameObject leyenda;
    public GameObject personajes;

    public GameObject easyButton;
    public GameObject normalButton;
    public GameObject heavyButton;

    public GameObject easyButtonHover;
    public GameObject normalButtonHover;
    public GameObject heavyButtonHover;

    // Start is called before the first frame update
    void Start()
    {
        easyButton.SetActive(false);
        normalButton.SetActive(true);
        heavyButton.SetActive(true);


        easyButtonHover.SetActive(true);
        normalButtonHover.SetActive(false);
        heavyButtonHover.SetActive(false);


        PlayerPrefs.SetFloat("minTimeAttack", 2.0f);
        PlayerPrefs.SetFloat("maxTimeAttack", 4.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void setLevel(int level)
    {
        Debug.Log("Level: " + level);
        if (level == 0)
        {
            easyButton.SetActive(false);
            normalButton.SetActive(true);
            heavyButton.SetActive(true);


            easyButtonHover.SetActive(true);
            normalButtonHover.SetActive(false);
            heavyButtonHover.SetActive(false);

            PlayerPrefs.SetFloat("minTimeAttack", 2.0f);
            PlayerPrefs.SetFloat("maxTimeAttack", 5.0f);
        }
        if (level == 1)
        {
            easyButton.SetActive(true);
            normalButton.SetActive(false);
            heavyButton.SetActive(true);


            easyButtonHover.SetActive(false);
            normalButtonHover.SetActive(true);
            heavyButtonHover.SetActive(false);

            normalButton.SetActive(false);
            normalButton.SetActive(false);
            normalButton.SetActive(false);
            normalButton.SetActive(false);

            normalButtonHover.SetActive(true);
            normalButtonHover.SetActive(true);
            normalButtonHover.SetActive(true);
            normalButtonHover.SetActive(true);

            PlayerPrefs.SetFloat("minTimeAttack", 1.0f);
            PlayerPrefs.SetFloat("maxTimeAttack", 2.0f);
        }
        if (level == 2)
        {
            easyButton.SetActive(true);
            normalButton.SetActive(true);
            heavyButton.SetActive(false);


            easyButtonHover.SetActive(false);
            normalButtonHover.SetActive(false);
            heavyButtonHover.SetActive(true);

            PlayerPrefs.SetFloat("minTimeAttack", 0.1f);
            PlayerPrefs.SetFloat("maxTimeAttack", 0.5f);
        }
        clickAudio.Play();
    }
    public void initMainMenuScene()
    {
        creditos.SetActive(false);
        principal.SetActive(true);
        leyenda.SetActive(false);
        personajes.SetActive(false);
        clickAudio.Play();
    }
    public void initAuthorMenuScene()
    {
        creditos.SetActive(true);
        principal.SetActive(false);
        leyenda.SetActive(false);
        clickAudio.Play();
        personajes.SetActive(false);
    }
    public void initLeyendsMenuScene()
    {
        creditos.SetActive(false);
        principal.SetActive(false);
        leyenda.SetActive(true);
        personajes.SetActive(false);
        clickAudio.Play();
    }
    public void initPlayersMenuScene()
    {
        creditos.SetActive(false);
        principal.SetActive(false);
        leyenda.SetActive(false);
        personajes.SetActive(true);
        clickAudio.Play();
    }
    public void initBoboScene()
    {
        clickAudio.Play();
        Debug.Log("Bobo");
        PlayerPrefs.SetString("playerName", "Bobo");
        PlayerPrefs.SetFloat("dangerPlayerRatio", 1.5f);
        PlayerPrefs.SetFloat("speedPlayerRatio", 0.5f);
        SceneManager.LoadScene("Lvl1Scene");
    }
    public void initTucusitoScene()
    {
        clickAudio.Play();
        Debug.Log("Tucusito");
        PlayerPrefs.SetString("playerName", "Tucusito");
        PlayerPrefs.SetFloat("dangerPlayerRatio", 1.0f);
        PlayerPrefs.SetFloat("speedPlayerRatio", 1.0f);
        SceneManager.LoadScene("Lvl1Scene");
    }
    public void doExitGame()
    {
        clickAudio.Play();
        Application.Quit();
    }
}
