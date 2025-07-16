using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManagerTwo : MonoBehaviour
{
    private Hero _heroHealth;
    private TextMeshPro _gameObjectText;
    private string _gameObjectPreviousText;
    [SerializeField] private CardsRepository _cardsRepository;
    [SerializeField] private UsedCardsRepository _usedCardsRepository;
    [SerializeField] private PathFunctionFollower2D _pathFunctionFollower2D;
    [SerializeField] private GameObject _hero;
    [SerializeField] private GameObject _toggleButton;
    [SerializeField] private GameObject _toggleButtontext;
    public int battleStep = 0;

    private bool _cardsHidden = false;

    // Получение скриптов игровых объектов.
    void Start()
    {
        _heroHealth = _hero.GetComponent<Hero>();
        _toggleButtontext = _toggleButton.transform.GetChild(0).gameObject;
    }
    public void InequalitiesCalculation()
    {
        string ResultFormula = "";
        bool isFunction = false;
        int CardCount = 0;
        _gameObjectPreviousText = "";
        //Объединение всех значений карт в одну строку уравнение.
        foreach (var UsedCard in _usedCardsRepository.UsedCards)
        {
            _gameObjectText = UsedCard.transform.GetChild(0).GetComponent<TextMeshPro>();
            CardCount += 1;
            if (_gameObjectText.text.Contains("()"))
            {
                ResultFormula = "(" + ResultFormula + ")" + _gameObjectText.text.Replace("()", "");
            }
            else if (_gameObjectText.text.Contains("√"))
            {
                ResultFormula = ResultFormula + "sqrt(";
                isFunction = true;
            }
            else if (isFunction && CardCount >= _usedCardsRepository.UsedCards.Count)
            {
                ResultFormula = ResultFormula + _gameObjectText.text + ")";
            }
            else if (_gameObjectText.text.Contains("x") && _gameObjectPreviousText.Contains("x"))
            {
                ResultFormula = ResultFormula + "*(" + _gameObjectText.text + ")";
            }
            else if (_gameObjectText.text.Contains("x") && int.TryParse(_gameObjectPreviousText, out _))
            {
                ResultFormula = ResultFormula + "*(" + _gameObjectText.text + ")";
            }
            else if (int.TryParse(_gameObjectText.text, out int x) && _gameObjectPreviousText.Contains("x") && x >= 0)
            {
                ResultFormula = ResultFormula + "*" + _gameObjectText.text;
            }
            else if (_gameObjectText.text.Contains("x"))
            {
                ResultFormula = ResultFormula + "(" + _gameObjectText.text + ")";
            }
            else
            {
                ResultFormula = ResultFormula + " " + _gameObjectText.text;
            }
            _gameObjectPreviousText = _gameObjectText.text;
        }

        _pathFunctionFollower2D.functionString = ResultFormula;
        _pathFunctionFollower2D.ShowMovement();
        _pathFunctionFollower2D.StartMovement();



    }
    // Функция на очистку всех карт перед следующим ходом игрока.
    public void NextMove()
    {
        foreach (var UsedCard in _usedCardsRepository.UsedCards)
        {
            Destroy(UsedCard);

        }
        _usedCardsRepository.UsedCards.Clear();
        foreach (var Card in _cardsRepository.Cards)
        {
            Destroy(Card);

        }
        _cardsRepository.Cards.Clear();
        battleStep += 1;
    }
    public void ShowPath()
    {
        CancelInvoke(nameof(HidePath));
        string ResultFormula = "";
        bool isFunction = false;
        int CardCount = 0;
        _gameObjectPreviousText = "";
        //Объединение всех значений карт в одну строку уравнение.
        foreach (var UsedCard in _usedCardsRepository.UsedCards)
        {
            _gameObjectText = UsedCard.transform.GetChild(0).GetComponent<TextMeshPro>();
            CardCount += 1;
            if (_gameObjectText.text.Contains("()"))
            {
                ResultFormula = "(" + ResultFormula + ")" + _gameObjectText.text.Replace("()", "");
            }
            else if (_gameObjectText.text.Contains("√"))
            {
                ResultFormula = ResultFormula + "sqrt(";
                isFunction = true;
            }
            else if (isFunction && CardCount >= _usedCardsRepository.UsedCards.Count)
            {
                ResultFormula = ResultFormula + _gameObjectText.text+ ")";
            }
            else if (_gameObjectText.text.Contains("x") && _gameObjectPreviousText.Contains("x"))
            {
                ResultFormula = ResultFormula + "*(" + _gameObjectText.text + ")";
            }
            else if (_gameObjectText.text.Contains("x") && int.TryParse(_gameObjectPreviousText, out _))
            {
                ResultFormula = ResultFormula + "*(" + _gameObjectText.text + ")";
            }
            else if (int.TryParse(_gameObjectText.text, out int x) && _gameObjectPreviousText.Contains("x") && x >= 0)
            {
                ResultFormula = ResultFormula + "*" + _gameObjectText.text;
            }
            else if (_gameObjectText.text.Contains("x"))
            {
                ResultFormula = ResultFormula + "(" + _gameObjectText.text + ")";
            }
            else
            {
                ResultFormula = ResultFormula + " " + _gameObjectText.text;
            }
            _gameObjectPreviousText = _gameObjectText.text;
        }
        _pathFunctionFollower2D.functionString = ResultFormula;
        _pathFunctionFollower2D.ShowMovement();
        Invoke(nameof(HidePath), 5f);
    }
    private void HidePath()
    {
        _pathFunctionFollower2D.HideMovement();
    }
    public void ToggleCards()
    {
        if (_cardsHidden)
        {
            ShowCards();    
            _cardsHidden = false;
            _toggleButtontext.GetComponent<TextMeshProUGUI>().text = "Скрыть карты";

        }
        else
        {
            HideCards();      
            _cardsHidden = true;
            _toggleButtontext.GetComponent<TextMeshProUGUI>().text = "Показать карты";
        }
    }
    private void ShowCards()
    {
        foreach (var usedCard in _usedCardsRepository.UsedCards)
        {
            usedCard.SetActive(true);
        }

        foreach (var card in _cardsRepository.Cards)
        {
            card.SetActive(true);
        }
    }
    private void HideCards()
    {
        foreach (var usedCard in _usedCardsRepository.UsedCards)
        {
            usedCard.SetActive(false);
        }

        foreach (var card in _cardsRepository.Cards)
        {
            card.SetActive(false);
        }
    }


}

