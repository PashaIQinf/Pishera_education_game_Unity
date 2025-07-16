using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class TutorialManager : MonoBehaviour
{
    public GameObject tutorialCanvas;
    public GameObject tutorialPanel;
    public TextMeshProUGUI tutorialText;
    public Button continueButton;
    public Button skipButton;
    public Button tutorialFightButton;
    public GenerateCards generateCards;
    public CardManager cardManager;
    public GenerateCardsTwo generateCardsTwo;
    public bool IsReference;
    private GameObject Cards;
    private GameObject FightButton;
    private int step = 0;
    private int Level;

    void Start()
    {
        Cards = GameObject.Find("Cards");
        FightButton = GameObject.Find("FightButton");
        Level = PlayerPrefs.GetInt("Level");
        if (Level <= 3)
        {
            tutorialFightButton.onClick.AddListener(delegate { Cards.GetComponent<CardManager>().InequalitiesCalculation(); });
            tutorialFightButton.onClick.AddListener(delegate { Cards.GetComponent<CardManager>().NextMove(); });
            tutorialFightButton.onClick.AddListener(delegate { OnClickButton(); });
            generateCards = Cards.GetComponent<GenerateCards>();
            cardManager = Cards.GetComponent<CardManager>();
        }
        else
        {
            generateCardsTwo = Cards.GetComponent<GenerateCardsTwo>();
        }
        tutorialFightButton.gameObject.SetActive(false);
        tutorialPanel.SetActive(true);
        ShowStep(step, Level);
    }

    public void ShowStep(int index, int level)
    {
        step = index;
        if ( level <= 3)
        {
            switch (step)
            {
                case 0:
                    tutorialText.text = "Добро пожаловать в 'Piщера'! Хотите узнать, как играть?";
                    skipButton.gameObject.SetActive(true);
                    FightButton.SetActive(false);
                    break;
                case 1:
                    tutorialText.text = "* Ты — ученик (слева), враг — справа.\n* Используй карты ниже, чтобы составить неравенство.";
                    skipButton.gameObject.SetActive(false);
                    continueButton.gameObject.transform.position = skipButton.gameObject.transform.position;
                    break;
                case 2:
                    tutorialText.text = "Пример выражения: 2 * x > 15.\nЕсли оно правильно — ты атакуешь врага!";
                    break;
                case 3:
                    tutorialText.text = "Ошибка в выражении — ты получаешь урон.\nВнимательно подбирай карты!";
                    break;
                case 4:
                    tutorialText.text = "Теперь попробуй составить выражение.\nКогда будешь готов — нажми 'Завершить ход'.";
                    generateCards.TutorialGenerateCard();
                    tutorialFightButton.gameObject.SetActive(true);
                    continueButton.gameObject.SetActive(false);
                    break;
                case 5:
                    tutorialText.text = "Отлично! Теперь ты готов к бою. Удачи, ученик!";
                    continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Начать игру!";
                    break;
            }
        }
        else
        {
            switch (step)
            {
                case 0:
                    tutorialText.text = "Хотите узнать, как играть на втором уровне?";
                    skipButton.gameObject.SetActive(true);
                    break;
                case 1:
                    tutorialText.text = "* Ты — ученик на лодке (слева) твоя главная цель проплыть реку.\n* Используй карты ниже, чтобы составить функцию.";
                    skipButton.gameObject.SetActive(false);
                    continueButton.gameObject.transform.position = skipButton.gameObject.transform.position;
                    break;
                case 2:
                    tutorialText.text = "Пример выражения: 2 * x + 5.\nПосле составление уравнения ты можешь посмотреть путь нажав кнопку 'Показать путь'";
                    break;
                case 3:
                    tutorialText.text = "Если тебе мешают карты при просмотре пути лодки.\nПросто нажми на кнопку 'Скрыть карты', при повторном нажатии снова прказывает карты.";
                    break;
                case 4:
                    tutorialText.text = "Если тебе путь устраивает\nТогда нажми на кнопку 'Завершить ход'.";
                    break;
                case 5:
                    tutorialText.text = "Отлично! Теперь ты готов к прохождению реки. Удачи, ученик!";
                    continueButton.GetComponentInChildren<TextMeshProUGUI>().text = "Начать игру!";
                    break;
            }
        }
    }

    public void OnContinue()
    {
        if (step == 3 && Level <= 3 && IsReference)
        {
            ShowStep(step = step + 2, Level);
        }
        else if (step < 5)
        {
            ShowStep(++step, Level);
        }
        else
        {
            FightButton.SetActive(true);
            Destroy(gameObject);
            if (Level <= 3 && !(IsReference))
            {
                generateCards.GenerateCard(); // вручную запускаем начальные карты
            }
            else if (!IsReference)
            {
                generateCardsTwo.GenerateCard();
            }
        }
    }

    public void OnSkip()
    {
        FightButton.SetActive(true);
        Destroy(gameObject);
        if (Level <= 3 && !IsReference)
        {
            generateCards.GenerateCard(); // запускаем карты сразу
        }
        else if(!IsReference)
        {
            generateCardsTwo.GenerateCard();
        }
    }
    public void OnClickButton()
    {
        continueButton.gameObject.SetActive(true);
    }
}
