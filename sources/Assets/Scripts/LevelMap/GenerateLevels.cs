using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Runtime.CompilerServices;
using UnityEngine.UI;

public class GenerateLevels : MonoBehaviour
{
    [SerializeField] private GameObject Level;
    [SerializeField] private GameObject Stage;
    [SerializeField] private Material lineMaterial;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    [SerializeField] private float levelY;
    [SerializeField] private float headerYOffset;
    [SerializeField] private float _offset;
    [SerializeField] private int _NumberLevels;
    [SerializeField] private int _LevelsPerStage;


    private int currentStageIndex = 0;
    private List<GameObject> stageGroups = new List<GameObject>();
    [SerializeField] Save save;

    private GameObject _gameObjectText;

    void Start()
    {
        string saveJson = PlayerPrefs.GetString("Save");
        save = JsonUtility.FromJson<Save>(saveJson);
        GenerateStages();
        UpdateStageVisibility();

        if (nextButton) nextButton.onClick.AddListener(NextStage);
        if (previousButton) previousButton.onClick.AddListener(PreviousStage);
    }
    // Инициализация функции генерирующая объекты уровни в зависимости от их числа.
    private void GenerateStages()
    {
        int numStages = Mathf.CeilToInt((float)_NumberLevels / _LevelsPerStage);
        int levelIndex = 0;

        for (int stageIndex = 0; stageIndex < numStages; stageIndex++)
        {
            // Создаём контейнер для этапа
            GameObject stageGroup = new GameObject($"Stage_{stageIndex + 1}");
            stageGroup.transform.SetParent(transform);
            stageGroups.Add(stageGroup);

            List<Vector3> levelPositions = new List<Vector3>();

            // Центрируем уровни в этом этапе
            float totalWidth = (_LevelsPerStage - 1) * _offset;
            float startX = -totalWidth / 2f;

            for (int i = 0; i < _LevelsPerStage && levelIndex < _NumberLevels; i++, levelIndex++)
            {
                float x = startX + i * _offset;
                Vector2 pos = new Vector2(x, levelY);

                GameObject level = Instantiate(Level, pos, Quaternion.identity, stageGroup.transform);
                level.name = $"LevelIcon {levelIndex}";

                var levelText = level.transform.GetChild(0).GetComponent<TextMeshPro>();
                levelText.text = $"Уровень {levelIndex + 1}";

                var levelCompleteText = level.transform.GetChild(1).GetComponent<TextMeshPro>();
                var levelSprite = level.GetComponent<SpriteRenderer>();
                if ((levelIndex + 1 <= save.Level) && save.Level != 0)
                {
                    levelCompleteText.text = "Пройден";
                    levelSprite.color = new Color(1, 0.7490196f,0);
                }
                else if (levelIndex + 1 <= save.Level + 1)
                {
                    levelCompleteText.text = "";
                }
                else
                {
                    levelCompleteText.text = "Недоступен";
                    levelSprite.color = new Color(0.7529412f, 0.7529412f, 0.7529412f);
                }

                levelPositions.Add(pos);
            }

            // Заголовок этапа — по центру между уровнями
            if (levelPositions.Count > 0)
            {
                int midIndex = levelPositions.Count / 2;
                Vector3 headerPos = levelPositions[midIndex] + new Vector3(0, headerYOffset, 0);
                GameObject header = Instantiate(Stage, headerPos, Quaternion.identity, stageGroup.transform);
                header.name = $"StageHeader {stageIndex + 1}";
                header.GetComponent<TextMeshPro>().text = $"Этап {stageIndex + 1}";
            }

            // Соединительная линия между уровнями
            if (levelPositions.Count > 1)
            {
                GameObject lineObj = new GameObject("ConnectionLine");
                lineObj.transform.parent = stageGroup.transform;

                LineRenderer lr = lineObj.AddComponent<LineRenderer>();
                lr.positionCount = levelPositions.Count;
                lr.SetPositions(levelPositions.ToArray());
                lr.material = lineMaterial;
                lr.widthMultiplier = 0.1f;
                lr.textureMode = LineTextureMode.Stretch;
                lr.sortingOrder = -1;
            }
        }
    }

    private void UpdateStageVisibility()
    {
        for (int i = 0; i < stageGroups.Count; i++)
        {
            stageGroups[i].SetActive(i == currentStageIndex);
        }

        // Обновляем состояние кнопок
        if (previousButton != null)
            previousButton.gameObject.SetActive(currentStageIndex > 0);

        if (nextButton != null)
            nextButton.gameObject.SetActive(currentStageIndex < stageGroups.Count - 1);
    }

    private void NextStage()
    {
        if (currentStageIndex < stageGroups.Count - 1)
        {
            currentStageIndex++;
            UpdateStageVisibility();
        }
    }

    private void PreviousStage()
    {
        if (currentStageIndex > 0)
        {
            currentStageIndex--;
            UpdateStageVisibility();
        }
    }
}
