using System;
using UnityEngine;
using TMPro;
using Random = UnityEngine.Random;
using System.Collections.Generic;

public class GenerateCards : MonoBehaviour
{
    public Transform spawnPos;
    public GameObject _gameObjectText, _gameObjectText2;
    [SerializeField] public GameObject Card;
    [SerializeField] public float _offset;
    [SerializeField] public int _NumberCard;
    [SerializeField] public CardsRepository _cardsRepository;
    [SerializeField] public UsedCardsRepository _usedCardsRepository;
    [SerializeField] public CardTypesRepository _cardTypesRepository;
    [SerializeField] private GameObject _endBattleImage;
    [SerializeField] private GameObject tutorialPrefab;
    public Sprite[] spriteArray;

    void Start()
    {
        int LevelStageOne = PlayerPrefs.GetInt("LevelEnemy");
        if (LevelStageOne == 0)
        {
            Instantiate(tutorialPrefab);
        }
        else
        {
            GenerateCard();
        }
    }
    // Функция генерация карт со случайной выборкой вида и значения в битве.
    public void GenerateCard()
    {
        _cardsRepository.Cards.Clear();
        List<int> test = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> randomList = new List<int>();
        for (; randomList.Count < 7;)
        {
            var random = Random.Range(0, test.Count);
            if (!randomList.Contains(random))
            {
                randomList.Add(random);
            }
        }
        var cardsize = Card.GetComponent<SpriteRenderer>().bounds.size;
        Card.GetComponent<Card>()._endBattleImage = _endBattleImage;
        // Генерация на каждой карты с случайным значением заданной случайным выборам вида. 
        for (int x = 0; x < _NumberCard; x++)
        {
                var position = new Vector3(((-0.44f * _NumberCard) +x) * cardsize.x + _offset, -7f,4f);
                var card = Instantiate(Card, position, Quaternion.identity);
                card.GetComponent<SpriteRenderer>().sprite = spriteArray[randomList[x]];
                card.name = $"Card {x}";
                _cardsRepository.Cards.Add(card);
                _gameObjectText = card.transform.GetChild(0).gameObject;
                _gameObjectText2 = card.transform.GetChild(1).gameObject;
            int randomNumber = Random.Range(0, _cardTypesRepository.ValueCardsOne.Count);
                string CardValue = _cardTypesRepository.ValueCardsOne[randomNumber];
                if (CardValue == "()^x")
                {
                   string Variable = $"{Random.Range(1, 3)}";
                   CardValue = CardValue.Replace("x", Variable);
                }
                else if (CardValue == "+")
                {
                   string Variable = $"{_cardTypesRepository.Operations[Random.Range(0, _cardTypesRepository.Operations.Count)]}";
                   CardValue = Variable;
                }
                else if (CardValue == "i")
                {
                  string Variable = $"{Random.Range(1, 9)}*x";
                  CardValue = Variable;
                }
                else if (CardValue == "n")
                {
                  string Variable = $"{Random.Range(1, 20)}";
                  CardValue = Variable;
                }
                if (_cardsRepository.Cards.Count == (_NumberCard - 1))
                {
                  string Variable = $"{_cardTypesRepository.ComparisonOperations[Random.Range(0, _cardTypesRepository.ComparisonOperations.Count)]}";
                  CardValue = Variable;
                }
                else if (_cardsRepository.Cards.Count == _NumberCard)
                {
                  string Variable = $"{Random.Range(1, 20)}";
                  CardValue = Variable;
                }
                else if (_cardsRepository.Cards.Count == (_NumberCard-(_NumberCard - 1)))
                {
                   string Variable = $"{Random.Range(1, 9)}*x";
                   CardValue = Variable;
                }
            _gameObjectText.GetComponent<TextMeshPro>().text = CardValue;
            _gameObjectText2.GetComponent<TextMeshPro>().sortingOrder = 0;
            if (CardValue.Contains("^"))
            {
                _gameObjectText.SetActive(false);
                _gameObjectText2.GetComponent<TextMeshPro>().sortingOrder = 3;
                if (CardValue == "()^1")
                {
                    _gameObjectText2.GetComponent<TextMeshPro>().text = CardValue.Replace($"^1", "¹");
                }
                else if (CardValue == "()^2")
                {
                    _gameObjectText2.GetComponent<TextMeshPro>().text = CardValue.Replace($"^2", "²");
                }
                else if (CardValue == "()^3")
                {
                    _gameObjectText2.GetComponent<TextMeshPro>().text = CardValue.Replace($"^3", "³");
                }
            }
        }
    }
    public void TutorialGenerateCard()
    {
        _cardsRepository.Cards.Clear();
        List<int> test = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        List<int> randomList = new List<int>();
        for (; randomList.Count < 7;)
        {
            var random = Random.Range(0, test.Count);
            if (!randomList.Contains(random))
            {
                randomList.Add(random);
            }
        }
        List<string> TutorialValues = new List<string>() { "2*x", ">", "15" };
        var cardsize = Card.GetComponent<SpriteRenderer>().bounds.size;
        Card.GetComponent<Card>()._endBattleImage = _endBattleImage;
        // Генерация на каждой карты с случайным значением заданной случайным выборам вида. 
        for (int x = 0; x < 3; x++)
        {
            var position = new Vector3(((-0.44f * 3) + x) * cardsize.x + _offset, -7f, 4f);
            var card = Instantiate(Card, position, Quaternion.identity);
            card.GetComponent<SpriteRenderer>().sprite = spriteArray[randomList[x]];
            card.name = $"Card {x}";
            _cardsRepository.Cards.Add(card);
            _gameObjectText = card.transform.GetChild(0).gameObject;
            _gameObjectText2 = card.transform.GetChild(1).gameObject;
            
            _gameObjectText.GetComponent<TextMeshPro>().text = TutorialValues[x];
            _gameObjectText2.GetComponent<TextMeshPro>().sortingOrder = 0;
            
        }
    }
}
