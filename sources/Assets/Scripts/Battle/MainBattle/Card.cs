using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.EventSystems;
using System.Linq;

public class Card : MonoBehaviour
{
    private GameObject _gameObjectText, _gameObjectText2;
    private Rigidbody2D _rigidbody;
    private Vector3 _offset, _originalPosition;
    private bool _dragging, _placed,_replace;
    public List<int> indexReplace = new List<int>();
    [SerializeField] private GameObject card;
    [SerializeField] private CardsRepository _cardsRepository;
    [SerializeField] private UsedCardsRepository _usedCardsRepository;
    [SerializeField] private CardTypesRepository _cardTypesRepository;
    [SerializeField] public GameObject _endBattleImage;
    [SerializeField] private AudioSource cardSource;
    [SerializeField] private AudioClip cardUpSource;
    [SerializeField] private AudioClip cardDownSource;

    void Awake()
    {
        // «апоминаем исходное положение карты.
        if (_placed) return;
        _originalPosition = transform.position;
    }

    //Ќазначение значений всех использующихс€ переменных при запуске битвы.
    void Start()
    {
        _cardsRepository = GameObject.FindObjectOfType<CardsRepository>();
        _usedCardsRepository = GameObject.FindObjectOfType<UsedCardsRepository>();
        _cardTypesRepository = GameObject.FindObjectOfType<CardTypesRepository>();

        _gameObjectText = transform.GetChild(0).gameObject;
        _gameObjectText2 = transform.GetChild(1).gameObject;
        _replace = false;
    }

    private void Update()
    {
        // ќбновление позиции карты на основании положени€ мыши при условии если карта не положена и выброшена.
        if (_placed) return;
        if (!_dragging) return;

        var mousePosition = GetMousePos();
        transform.position = mousePosition - _offset;
        
    }

    void OnMouseDown()
    {
            if (_placed || _endBattleImage.activeSelf) return;
            _dragging = true;
            _offset = GetMousePos() - (Vector3)transform.position;

            var _drag = card.GetComponent<SpriteRenderer>();
            _drag.sortingOrder = 4;

            var _dragtext = _gameObjectText.GetComponent<TextMeshPro>();
            _dragtext.sortingOrder = 5;
            if (_dragtext.text.Contains("^"))
            {
               var _dragtext2 = _gameObjectText2.GetComponent<TextMeshPro>();
               _dragtext2.sortingOrder = 5;
            }
        cardSource.PlayOneShot(cardUpSource);
    }

    void OnMouseUp()
    {
        if (_endBattleImage.activeSelf) return;
        // ѕомещение карты на игровое поле и добавление в массив с использующиес€ картами.
        if (((transform.position.y > -3.7f && transform.position.y < 4.45f) && (transform.position.x > -8.8f && transform.position.x < 8.6f)) && !_placed)
        {
            _placed = true; 
            _cardsRepository.Cards.Remove(card);
            _usedCardsRepository.UsedCards.Add(card);
            var cardsize = card.GetComponent<SpriteRenderer>().bounds.size;
            int x = 0;
            foreach (var count in _usedCardsRepository.UsedCards)
            {
                count.transform.position = new Vector3(((-0.475f * _usedCardsRepository.UsedCards.Count) + x) * cardsize.x + 1, 0, -4);
                x = x + 1;
            }
            cardSource.PlayOneShot(cardDownSource);

        } 
        else
        {
            // ≈сли нажали на нижнюю часть карты, то убирает из игрового пол€ и массива использующихс€ карт.
            if ((GetMousePos().y < transform.position.y) || !_placed)
            {
                transform.position = _originalPosition;
                _dragging = false;
                _placed = false;
                _usedCardsRepository.UsedCards.Remove(card);
                if (_cardsRepository.Cards.Count(item => item == card) == 0)
                {
                    _cardsRepository.Cards.Add(card);
                }
                var cardsize = card.GetComponent<SpriteRenderer>().bounds.size;
                int x = 0;
                foreach (var count in _usedCardsRepository.UsedCards)
                {
                    count.transform.position = new Vector3(((-0.475f * _usedCardsRepository.UsedCards.Count) + x) * cardsize.x + 1, 0, -4);
                    x = x + 1;
                }
                _usedCardsRepository.ReplaceCards.Clear();
                cardSource.PlayOneShot(cardDownSource);
            }
            // ≈сли нажали на верхнюю часть карты, то приподнимает еЄ и если выбрать вторую то мен€ет ее местами.
            else
            {
                transform.position = new Vector3(transform.position.x, 0.5f, -4);
                _usedCardsRepository.ReplaceCards.Add(card);
                if (_usedCardsRepository.ReplaceCards.Count == 2)
                {
                    if (_usedCardsRepository.ReplaceCards[0] == _usedCardsRepository.ReplaceCards[1])
                    {
                        _usedCardsRepository.ReplaceCards.Clear();
                    }
                    foreach (var count in _usedCardsRepository.UsedCards)
                    {
                        if (count == _usedCardsRepository.ReplaceCards[0])
                        {
                            indexReplace.Add(_usedCardsRepository.UsedCards.FindIndex(a => a == count));
                        }
                        else if (count == _usedCardsRepository.ReplaceCards[1])
                        {
                            indexReplace.Add(_usedCardsRepository.UsedCards.FindIndex(a => a == count));
                        }
                    }
                    if (_usedCardsRepository.ReplaceCards[0].transform.position.x < _usedCardsRepository.ReplaceCards[1].transform.position.x)
                    {
                        _usedCardsRepository.UsedCards[indexReplace[0]] = _usedCardsRepository.ReplaceCards[1];
                        _usedCardsRepository.UsedCards[indexReplace[1]] = _usedCardsRepository.ReplaceCards[0];
                    }
                    else if (_usedCardsRepository.ReplaceCards[0].transform.position.x > _usedCardsRepository.ReplaceCards[1].transform.position.x)
                    {
                        _usedCardsRepository.UsedCards[indexReplace[0]] = _usedCardsRepository.ReplaceCards[0];
                        _usedCardsRepository.UsedCards[indexReplace[1]] = _usedCardsRepository.ReplaceCards[1];
                    }
                    indexReplace.Clear();
                    _usedCardsRepository.ReplaceCards.Clear();
                    var cardsize = card.GetComponent<SpriteRenderer>().bounds.size;
                    int i = 0;
                    foreach (var count in _usedCardsRepository.UsedCards)
                    {
                        count.transform.position = new Vector3(((-0.475f * _usedCardsRepository.UsedCards.Count) + i) * cardsize.x + 1, 0, -4);
                        i = i + 1;
                    }
                }
                cardSource.PlayOneShot(cardUpSource);
            }
        }


        var _drag = card.GetComponent<SpriteRenderer>();
        _drag.sortingOrder = 2;

        var _dragtext = _gameObjectText.GetComponent<TextMeshPro>();
        _dragtext.sortingOrder = 3;
        if (_dragtext.text.Contains("^"))
        {
            var _dragtext2 = _gameObjectText2.GetComponent<TextMeshPro>();
            _dragtext2.sortingOrder = 3;
        }

    }

    // Ћокальна€ функци€ возвращающа€ координаты мышки в виде вектора.
    Vector3 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


}
