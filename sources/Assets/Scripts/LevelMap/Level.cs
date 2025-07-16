using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Level : MonoBehaviour
{
    [SerializeField] private GameObject LevelIcon;
    [SerializeField] private AudioSource levelSource;
    [SerializeField] private AudioClip audioClick;
    private TextMeshPro LevelComplete;

    private void Start()
    {
        LevelComplete = LevelIcon.transform.GetChild(1).GetComponent<TextMeshPro>();
    }
    // Функция загружающая уровень игры в зависимости от номера уровня.
    void OnMouseDown()
    {
        levelSource.PlayOneShot(audioClick);
        int result = int.Parse(Regex.Match(LevelIcon.name, @"\d+").Value);
        if (result <= 2 && LevelComplete.text != "Недоступен")
        {
            PlayerPrefs.SetInt("LevelEnemy", result);
            PlayerPrefs.SetInt("Level", result + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if (result > 2 && result <= 5 && LevelComplete.text != "Недоступен")
        {
            PlayerPrefs.SetInt("Level", result + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }
}
