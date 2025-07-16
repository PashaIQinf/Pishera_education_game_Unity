using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class CardManager : MonoBehaviour
{
    private Enemy _enemyHealth;
    private Hero _heroHealth;
    private TextMeshPro _gameObjectText;
    private string _gameObjectPreviousText;
    [SerializeField] private CardsRepository _cardsRepository;
    [SerializeField] private UsedCardsRepository _usedCardsRepository;
    [SerializeField] private GameObject _enemy;
    [SerializeField] private GameObject _hero;
    [SerializeField] private AudioSource enemySource;
    [SerializeField] private AudioSource heroSource;
    [SerializeField] private AudioClip audioAttack;
    public int battleStep = 0;

    // Получение скриптов игровых объектов.
    void Start()
    {
        _enemyHealth = _enemy.GetComponent<Enemy>();
        _heroHealth = _hero.GetComponent<Hero>();
    }
    public void InequalitiesCalculation()
    {
        string ResultFormula = "";
        int CountOperator = 0;
        string OperatorFormula = "";
        string Result = "";
        string ResultTwo = "";
        float answer = 0;
        string Operator = "";
        _gameObjectPreviousText = "";
        //Объединение всех значений карт в одну строку уравнение.
        foreach (var UsedCard in _usedCardsRepository.UsedCards)
        {
            _gameObjectText = UsedCard.transform.GetChild(0).GetComponent<TextMeshPro>();
            if (_gameObjectText.text == "<" || _gameObjectText.text == ">" || CountOperator >= 1) {

                CountOperator = CountOperator + 1;
                if (CountOperator > 1)
                {
                    if (_gameObjectText.text.Contains("()"))
                    {OperatorFormula = "(" + OperatorFormula + ")" + _gameObjectText.text.Replace("()", "");}
                    else if (_gameObjectText.text.Contains("x") && _gameObjectPreviousText.Contains("x"))
                    {OperatorFormula = OperatorFormula + "*(" + _gameObjectText.text + ")";}
                    else if (_gameObjectText.text.Contains("x") && int.TryParse(_gameObjectPreviousText, out _))
                    {OperatorFormula = OperatorFormula + "*(" + _gameObjectText.text + ")";}
                    else if (int.TryParse(_gameObjectText.text, out _) && _gameObjectPreviousText.Contains("x"))
                    {OperatorFormula = OperatorFormula + "*" + _gameObjectText.text;}
                    else if(_gameObjectText.text.Contains("x"))
                    {OperatorFormula = OperatorFormula + "(" + _gameObjectText.text + ")";}
                    else
                    {OperatorFormula = OperatorFormula + " " + _gameObjectText.text;}
                }

            }
            else
            {
                if (_gameObjectText.text.Contains("()"))
                {
                    ResultFormula = "(" + ResultFormula + ")" + _gameObjectText.text.Replace("()", "");
                }
                else if (_gameObjectText.text.Contains("x") && _gameObjectPreviousText.Contains("x"))
                {
                    ResultFormula = ResultFormula + "*(" + _gameObjectText.text + ")";
                }
                else if (_gameObjectText.text.Contains("x") && int.TryParse(_gameObjectPreviousText, out _))
                {
                    ResultFormula = ResultFormula + "*(" + _gameObjectText.text + ")";
                }
                else if (int.TryParse(_gameObjectText.text, out _) && _gameObjectPreviousText.Contains("x"))
                {
                    ResultFormula = ResultFormula + "*" + _gameObjectText.text;
                }
                else if(_gameObjectText.text.Contains("x"))
                {
                    ResultFormula = ResultFormula + "(" + _gameObjectText.text + ")";
                }
                else
                {
                    ResultFormula = ResultFormula + " " + _gameObjectText.text;
                }
            }
            if (_gameObjectText.text == "<" || _gameObjectText.text == ">" )
            {

                Operator = _gameObjectText.text;

            }
            _gameObjectPreviousText = _gameObjectText.text;

        }
        // Определение типа уравнение по знаку и вычисление наименьшего значение x из условий уравнения.
        if (Operator == "<")
        {
            float FinalResult = 0;
            float FinalResultTwo = 0;
            float i = 0.00f;
            if (OperatorFormula.Contains("x") && !(ResultFormula.Contains("x")))
            {
                int IndexX = OperatorFormula.IndexOf("x");
                int IndexX2 = OperatorFormula.IndexOf("x", IndexX+1);
                int IndexDivision = OperatorFormula.IndexOf("/");
                if (IndexX > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else if (IndexX2 > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else
                {
                    float.TryParse(ResultFormula, out float myInt);
                    while (FinalResult <= myInt)
                    {
                        Result = OperatorFormula.Replace("x", i.ToString());
                        ExpressionEvaluator.Evaluate(Result, out FinalResult);
                        string NextResult = ResultFormula.Replace("x", (i + 1).ToString());
                        ExpressionEvaluator.Evaluate(NextResult, out float FinalNextResult);
                        answer = FinalResult;
                        if (((answer == 0) || (answer == myInt)) && (i != 0))
                        {
                            if (FinalNextResult < myInt)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else if (FinalNextResult == 0)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else
                            {
                                _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                                enemySource.PlayOneShot(audioAttack);
                            }
                                break;
                        }
                        else if (answer > myInt)
                        {
                            _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                            enemySource.PlayOneShot(audioAttack);
                            break;
                        }
                        i++;
                    }
                }


            }
            else if (!(OperatorFormula.Contains("x")) && (ResultFormula.Contains("x")))
            {
                int IndexX = ResultFormula.IndexOf("x");
                int IndexX2 = ResultFormula.IndexOf("x",IndexX+1);
                int IndexDivision = ResultFormula.IndexOf("/");
                if (IndexX > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else if (IndexX2 > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else
                {
                    float.TryParse(OperatorFormula, out float myInt);
                    while (FinalResult <= myInt)
                    {
                        Result = ResultFormula.Replace("x", i.ToString());
                        ExpressionEvaluator.Evaluate(Result, out FinalResult);
                        string NextResult = ResultFormula.Replace("x", (i + 1).ToString());
                        ExpressionEvaluator.Evaluate(NextResult, out float FinalNextResult);
                        if (FinalResult < myInt)
                        {
                            answer = FinalResult;
                        }
                        if (((answer == 0) || (answer == myInt)) && (i != 0))
                        {
                            if (FinalNextResult > myInt)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else if (FinalNextResult == 0)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else
                            {
                                _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                                enemySource.PlayOneShot(audioAttack);
                            }
                            break;
                        }
                        else if (answer > 0 && answer < myInt)
                        {
                            _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                            enemySource.PlayOneShot(audioAttack);
                            break;
                        }
                        i++;
                    }
                }

            }
            else if (OperatorFormula.Contains("x") && (ResultFormula.Contains("x")))
            {
                
                    Result = ResultFormula.Replace("x", "2");
                    ResultTwo = OperatorFormula.Replace("x", "2");
                    ExpressionEvaluator.Evaluate(Result, out FinalResult);
                    ExpressionEvaluator.Evaluate(ResultTwo, out FinalResultTwo);
                    if (FinalResult < FinalResultTwo)
                    {
                        _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                        enemySource.PlayOneShot(audioAttack);
                    }
                    else
                    {
                        _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                        heroSource.PlayOneShot(audioAttack);
                      
                    }
            }
            else
            {
                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                heroSource.PlayOneShot(audioAttack);
            }
        }
        else if (Operator == ">")
        {
            float FinalResult = 0;
            float FinalResultTwo = 0;
            float i = 0;
            if (OperatorFormula.Contains("x") && !(ResultFormula.Contains("x")))
            {
                int IndexX = OperatorFormula.IndexOf("x");
                int IndexX2 = OperatorFormula.IndexOf("x", IndexX+1);
                int IndexDivision = OperatorFormula.IndexOf("/");
                if (IndexX > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else if (IndexX2 > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else
                {
                    float.TryParse(ResultFormula, out float myInt);
                    while (answer <= myInt)
                    {
                        Result = OperatorFormula.Replace("x", i.ToString());
                        ExpressionEvaluator.Evaluate(Result, out FinalResult);
                        string NextResult = ResultFormula.Replace("x", (i + 1).ToString());
                        ExpressionEvaluator.Evaluate(NextResult, out float FinalNextResult);
                        if (FinalResult < (float)myInt)
                        {
                            answer = FinalResult;
                        }
                        if (((answer == 0) || (answer == myInt)) && (i != 0))
                        {
                            if (FinalNextResult > myInt)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else if (FinalNextResult == 0)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else
                            {
                                _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                                enemySource.PlayOneShot(audioAttack);
                            }
                            break;
                        }
                        else if (answer > 0 && answer < myInt)
                        {
                            _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                            enemySource.PlayOneShot(audioAttack);
                            break;
                        }
                        i++;


                    }
                }

            }
            else if (!(OperatorFormula.Contains("x")) && (ResultFormula.Contains("x")))
            {
                int IndexX = ResultFormula.IndexOf("x");

                int IndexX2 = ResultFormula.IndexOf("x", IndexX+1);
                int IndexDivision = ResultFormula.IndexOf("/");
                if (IndexX > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else if (IndexX2 > IndexDivision && IndexDivision != -1)
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);
                }
                else
                {
                    float.TryParse(OperatorFormula, out float myInt);
                    while (answer <= myInt)
                    {
                        Result = ResultFormula.Replace("x", i.ToString());
                        ExpressionEvaluator.Evaluate(Result, out FinalResult);
                        string NextResult = ResultFormula.Replace("x", (i + 1).ToString());
                        ExpressionEvaluator.Evaluate(NextResult, out float FinalNextResult);
                        answer = FinalResult;
                        if (((answer == 0) || (answer == myInt)) && (i != 0))
                        {
                            if (FinalNextResult < myInt)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else if (FinalNextResult == 0)
                            {
                                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                                heroSource.PlayOneShot(audioAttack);
                            }
                            else
                            {
                                _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                                enemySource.PlayOneShot(audioAttack);
                            }
                            break;
                        }
                        else if (answer > myInt)
                        {
                            _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                            enemySource.PlayOneShot(audioAttack);
                            break;
                        }
                        i++;


                    }
                }
                

            }
            else if (OperatorFormula.Contains("x") && (ResultFormula.Contains("x")))
            {
                Result = ResultFormula.Replace("x", "2");
                ResultTwo = OperatorFormula.Replace("x", "2");
                ExpressionEvaluator.Evaluate(Result, out FinalResult);
                ExpressionEvaluator.Evaluate(ResultTwo, out FinalResultTwo);
                if (FinalResult > FinalResultTwo)
                {
                    _enemyHealth.ReceiveDamage(_enemyHealth.EnemyHealthFull * 0.1f);
                    enemySource.PlayOneShot(audioAttack);
                }
                else
                {
                    _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                    heroSource.PlayOneShot(audioAttack);

                }
            }
            else
            {
                _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
                heroSource.PlayOneShot(audioAttack);
            }

        }
        else
        {
            _heroHealth.ReceiveDamage(_enemyHealth.EnemyDamage);
            heroSource.PlayOneShot(audioAttack);
        }

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

}

