using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuEngine : MonoBehaviour
{
    public GameObject principal;
    public GameObject creditos;
    public GameObject leyenda;
    public GameObject personajes;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void initMainMenuScene()
    {
        creditos.SetActive(false);
        principal.SetActive(true);
        leyenda.SetActive(false);
        personajes.SetActive(false);
    }
    public void initAuthorMenuScene()
    {
        creditos.SetActive(true);
        principal.SetActive(false);
        leyenda.SetActive(false);
        personajes.SetActive(false);
    }
    public void initLeyendsMenuScene()
    {
        creditos.SetActive(false);
        principal.SetActive(false);
        leyenda.SetActive(true);
        personajes.SetActive(false);
    }
    public void initPlayersMenuScene()
    {
        creditos.SetActive(false);
        principal.SetActive(false);
        leyenda.SetActive(false);
        personajes.SetActive(true);
    }
    public void initBoboScene()
    {
        Debug.Log("Bobo");
        PlayerPrefs.SetString("playerName", "Bobo");
        PlayerPrefs.SetFloat("dangerPlayerRatio", 1.5f);
        PlayerPrefs.SetFloat("speedPlayerRatio", 0.5f);
        SceneManager.LoadScene("Lvl1Scene");
    }
    public void initTucusitoScene()
    {
        Debug.Log("Tucusito");
        PlayerPrefs.SetString("playerName", "Tucusito");
        PlayerPrefs.SetFloat("dangerPlayerRatio", 1.0f);
        PlayerPrefs.SetFloat("speedPlayerRatio", 1.0f);
        SceneManager.LoadScene("Lvl1Scene");
    }
    public void doExitGame()
    {
        Application.Quit();
    }
}
