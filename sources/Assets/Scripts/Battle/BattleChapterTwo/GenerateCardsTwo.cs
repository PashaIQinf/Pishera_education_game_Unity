using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

public class GenerateCardsTwo : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject _gameObjectText, _gameObjectText2;
    [SerializeField] public GameObject Card;
    [SerializeField] public float _offset;
    [SerializeField] public int _NumberCard;
    [SerializeField] public CardsRepository _cardsRepository;
    [SerializeField] public UsedCardsRepository _usedCardsRepository;
    [SerializeField] public CardTypesRepositoryTwo _cardTypesRepository;
    [SerializeField] public GenerateObstacles _generateObstacles;
    [SerializeField] private GameObject _endBattleImage;
    [SerializeField] private GameObject tutorialPrefab;
    public Sprite[] spriteArray;


    void Start()
    {
        int LevelStageTwo = PlayerPrefs.GetInt("Level");
        if (LevelStageTwo == 4)
        {
            Instantiate(tutorialPrefab);
        }
        else
        {
            GenerateCard();
        }
    }
    private List<string> GetCardPartsFromFormula(string formula)
    {
        List<string> parts = new List<string>();

        var matchPower = Regex.Match(formula, @"\(([\-]?[0-9.]+)\+x\)\^([0-9.]+)\/([0-9.]+)");
        if (matchPower.Success)
        {
            //(r + x)^n / m
            parts.Add(matchPower.Groups[1].Value);                  // r
            parts.Add("+");
            parts.Add("x");
            parts.Add("()^" + matchPower.Groups[2].Value);            // ^n
            parts.Add("/");
            parts.Add(matchPower.Groups[3].Value);                  // /m
            return parts;
        }
        var matchSqrt = Regex.Match(formula, @"sqrt\(([\-]?[0-9.]+)\*x\+\(([\-]?[0-9.]+)\)\)");
        if (matchSqrt.Success)
        {
            parts.Add("√"); // или "sqrt", если не используешь символ корня в UI
            parts.Add($"{matchSqrt.Groups[1].Value}*x");
            parts.Add("+");
            parts.Add(matchSqrt.Groups[2].Value);
            return parts;
        }
        var matchLinear = Regex.Match(formula, @"([\-]?[0-9.]+)\*x\+\(([\-]?[0-9.]+)\)");
        if (matchLinear.Success)
        {
            parts.Add($"{matchLinear.Groups[1].Value}*x");          // коэффициент * x
            parts.Add("+");
            parts.Add(matchLinear.Groups[2].Value);                 // сдвиг
            return parts;
        }

        return parts;
    }

    // Функция генерация карт со случайной выборкой вида и значения в битве.
    public void GenerateCard()
    {
        _cardsRepository.Cards.Clear();

        // Получаем нужные значения из формулы
        List<string> formulaParts = GetCardPartsFromFormula(_generateObstacles.Formula);
        List<string> finalCardValues = new List<string>(formulaParts);

        // Добавим случайные карточки-дополнения
        while (finalCardValues.Count < _NumberCard)
        {
            string extra = _cardTypesRepository.ValueCardsOne[Random.Range(0, _cardTypesRepository.ValueCardsOne.Count)];

            // Обработка шаблонов
            if (extra == "()^x")
            {
                extra = $"()^{Random.Range(1, 3)}";
            }
            else if (extra == "+")
            {
                extra = _cardTypesRepository.Operations[Random.Range(0, _cardTypesRepository.Operations.Count)];
            }
            else if (extra == "i")
            {
                extra = $"{Random.Range(1, 9)}*x";
            }
            else if (extra == "n")
            {
                extra = $"{Random.Range(1, 20)}";
            }

            finalCardValues.Add(extra);
        }

        // Перемешиваем карточки
        finalCardValues = finalCardValues.OrderBy(x => Random.value).ToList();
        var cardsize = Card.GetComponent<SpriteRenderer>().bounds.size;
        Card.GetComponent<Card>()._endBattleImage = _endBattleImage;
        // Генерация на каждой карты с случайным значением заданной случайным выборам вида. 
        for (int x = 0; x < _NumberCard; x++)
        {
            var position = new Vector3(((-0.44f * _NumberCard) + x) * cardsize.x + _offset, -7f, -4);
            var card = Instantiate(Card, position, Quaternion.identity);
            card.GetComponent<SpriteRenderer>().sprite = spriteArray[Random.Range(0, spriteArray.Length)];
            card.name = $"Card {x}";
            _cardsRepository.Cards.Add(card);

            string CardValue = finalCardValues[x];
            _gameObjectText = card.transform.GetChild(0).gameObject;
            _gameObjectText2 = card.transform.GetChild(1).gameObject;

            _gameObjectText.GetComponent<TextMeshPro>().text = CardValue;
            _gameObjectText2.GetComponent<TextMeshPro>().sortingOrder = 0;

            if (CardValue.Contains("^"))
            {
                _gameObjectText.SetActive(false);
                _gameObjectText2.GetComponent<TextMeshPro>().sortingOrder = 3;

                if (CardValue == "()^1")
                {
                    _gameObjectText2.GetComponent<TextMeshPro>().text = "()¹";
                }
                else if (CardValue == "()^2")
                {
                    _gameObjectText2.GetComponent<TextMeshPro>().text = "()²";
                }
                else if (CardValue == "()^3")
                {
                    _gameObjectText2.GetComponent<TextMeshPro>().text = "()³";
                }
            }
        }
    }
}
