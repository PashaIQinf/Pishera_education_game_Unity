using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattle : MonoBehaviour
{
    private Enemy _enemyHealth;
    private Hero _heroHealth;
    private GameObject _endBattleMenu;
    private GameObject _nextLevelButton;
    private GameObject _returnButton;
    private GameObject _endBattleName;
    private GameObject _levelScore;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _hero;
    [SerializeField] private GameObject _endBattleImage;
    [SerializeField] private CardManager _cardManager;
    private TextMeshProUGUI _nextLevelButtonText;
    private TextMeshProUGUI _endBattleNameText;
    private TextMeshProUGUI _levelScoreText;
    private int LevelEnemy;
    private int Level;

    private int MainScore;
    private int CompleteOnStep;
    private int BonusScore;

    [SerializeField] Save save;
    private SaveGameData saveGameData = new SaveGameData();

    // Получение скриптов игровых объектов.
    void Start()
    {
        string saveJson = PlayerPrefs.GetString("Save");
        save = JsonUtility.FromJson<Save>(saveJson);
        _enemyHealth = _enemy.GetComponent<Enemy>();
        _heroHealth = _hero.GetComponent<Hero>();
        _endBattleMenu = _endBattleImage.transform.GetChild(0).gameObject;
        _endBattleName = _endBattleMenu.transform.GetChild(0).gameObject;
        _levelScore = _endBattleMenu.transform.GetChild(1).gameObject;
        _nextLevelButton = _endBattleMenu.transform.GetChild(2).gameObject;
        _returnButton = _endBattleMenu.transform.GetChild(3).gameObject;
        _nextLevelButtonText = _nextLevelButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _endBattleNameText = _endBattleName.GetComponent<TextMeshProUGUI>();
        _levelScoreText = _levelScore.GetComponent<TextMeshProUGUI>();
        LevelEnemy = PlayerPrefs.GetInt("LevelEnemy");
        Level = PlayerPrefs.GetInt("Level");

}
    //Инициализации функции вывода результата битвы на экран с задержкой.
    void FixedUpdate()
    {
        if (!_endBattleImage.activeSelf)
        {
            ResultBattle();
        }
    }
    // Функция вывода результата битвы на экран с задержкой.
    private void ResultBattle()
    {
        if (_enemyHealth.EnemyHealth <= 1)
        {
            _endBattleNameText.text = "Уровень Пройден";
            _endBattleImage.SetActive(true);
            _endBattleMenu.SetActive(true);
            if (_heroHealth.HeroHealth / 10 == 10)
            {
                BonusScore = 500;
            }
            else
            {
                BonusScore = 0;
            }
            CompleteOnStep = _cardManager.battleStep;
            MainScore = (int)((_heroHealth.HeroHealth / 10) * 500) + (int)((10 / CompleteOnStep) * 500) + BonusScore;
            _levelScoreText.text = $"Общие очки: {MainScore}\nСделал ходов: {CompleteOnStep}\nДополнительные очки: {BonusScore}";
            if (save.Level != Level)
            {
                save.Level += 1;
                saveGameData.SaveData(save);
            }
        }
        else if (_heroHealth.HeroHealth <= 1)
        {
            _endBattleNameText.text = "Поражение";
            _endBattleImage.SetActive(true);
            _endBattleMenu.SetActive(true);
            _nextLevelButtonText.text = "Перезагрузить";
        }
    }
    public void OnNextLevelButton()
    {
        if (Level <= 2 && _enemyHealth.EnemyHealth < 1)
        {
            PlayerPrefs.SetInt("LevelEnemy", LevelEnemy + 1);
            PlayerPrefs.SetInt("Level", Level + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if (Level == 3 && _enemyHealth.EnemyHealth < 1)
        {
            PlayerPrefs.SetInt("Level", Level + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
