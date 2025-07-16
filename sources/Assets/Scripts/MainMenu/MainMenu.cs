using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Функция перехода из главного меню в  меню уровней.
    public void PLayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    // Функция выхода из игры.
    public void ExitGame()
    {
        Debug.Log("Игра закрылась");
        Application.Quit();
    }
    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
