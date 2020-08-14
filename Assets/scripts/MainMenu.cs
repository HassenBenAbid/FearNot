using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private GameObject market;

    private void OnEnable()
    {
        SaveSystem.load();
        market.SetActive(false);
    }

    public void start()
    {
        SceneManager.LoadScene(1);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void openMarket()
    {
        market.SetActive(true);
    }
}
