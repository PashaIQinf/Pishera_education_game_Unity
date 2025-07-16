using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleTwo : MonoBehaviour
{
    private Hero _heroHealth;
    private GameObject _endBattleMenu;
    private GameObject _nextLevelButton;
    private GameObject _returnButton;
    private GameObject _endBattleName;
    private GameObject _levelScore;
    [SerializeField] private GameObject _hero;
    [SerializeField] private GameObject _endBattleImage;
    [SerializeField] private CardManagerTwo _cardManagerTwo;
    private TextMeshProUGUI _nextLevelButtonText;
    private TextMeshProUGUI _endBattleNameText;
    private TextMeshProUGUI _levelScoreText;
    private int LevelEnemy;
    private int Level;
    private RectTransform _returnButtonRect;

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
        _heroHealth = _hero.GetComponent<Hero>();
        _endBattleMenu = _endBattleImage.transform.GetChild(0).gameObject;
        _endBattleName = _endBattleMenu.transform.GetChild(0).gameObject;
        _levelScore = _endBattleMenu.transform.GetChild(1).gameObject;
        _nextLevelButton = _endBattleMenu.transform.GetChild(2).gameObject;
        _returnButton = _endBattleMenu.transform.GetChild(3).gameObject;
        _nextLevelButtonText = _nextLevelButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        _endBattleNameText = _endBattleName.GetComponent<TextMeshProUGUI>();
        _levelScoreText = _levelScore.GetComponent<TextMeshProUGUI>();
        _returnButtonRect = _returnButton.GetComponent<RectTransform>();
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
        if (_hero.transform.position.x > 10 && Level == 6)
        {
            _endBattleNameText.text = "Уровень Пройден";
            _endBattleImage.SetActive(true);
            _endBattleMenu.SetActive(true);
            _nextLevelButton.SetActive(false);
            if (_heroHealth.HeroHealth / 10 == 10)
            {
                BonusScore = 500;
            }
            else
            {
                BonusScore = 0;
            }
            CompleteOnStep = _cardManagerTwo.battleStep;
            MainScore = (int)((_heroHealth.HeroHealth / 10) * 500) + (int)((1 / CompleteOnStep) * 500) + BonusScore;
            _levelScoreText.text = $"Общие очки: {MainScore}\nСделал ходов: {CompleteOnStep}\nДополнительные очки:{BonusScore}";
            _returnButtonRect.localPosition = _returnButtonRect.localPosition + new Vector3(240,0);
            save.Level += 1;
            saveGameData.SaveData(save);
        }
        else if (_hero.transform.position.x > 10)
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
            CompleteOnStep = _cardManagerTwo.battleStep;
            MainScore = (int)((_heroHealth.HeroHealth / 10) * 500) + (int)((1 / CompleteOnStep) * 500) + BonusScore;
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
        if (Level > 3 && _hero.transform.position.x > 10)
        {
            PlayerPrefs.SetInt("Level", Level + 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
