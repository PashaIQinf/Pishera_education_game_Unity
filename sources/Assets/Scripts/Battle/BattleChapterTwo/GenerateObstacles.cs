using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;
using CultureInfo = System.Globalization.CultureInfo;
using System;

public class GenerateObstacles : MonoBehaviour
{
    [SerializeField] private Transform spawnPos;
    [SerializeField] private GameObject Obstacle;
    [SerializeField] private SpawnHero _spawnHero;

    [Tooltip("Ширина пути реки")]
    [SerializeField] public float SizeGap;

    [SerializeField] public string Formula;

    void Start()
    {
        GenerateObstacle();
        _spawnHero.SpawnPlayer(Formula, SizeGap);
    }

    private void GenerateObstacle()
    {
        int FormulaType = PlayerPrefs.GetInt("Level");
        if (FormulaType == 4)
        {
            bool valid = false;

            double LineConstant = 0;
            double LineNumber = 0;
            while (!valid)
            {
                // Случайным образом выбираем LineConstant в безопасном диапазоне, чтобы избежать неправильной генерации
                LineConstant = Math.Round(Random.Range(-9, 9) * 0.1d, 1);

                // Вычислить допустимый диапазон для LineNumber
                float LineNumberMin = Mathf.Max(10 * (float)LineConstant - 9, -10 * (float)LineConstant - 9);
                float LineNumberMax = Mathf.Min(10 * (float)LineConstant + 9, -10 * (float)LineConstant + 9);

                if (LineNumberMin <= LineNumberMax && LineConstant != 0f)
                {
                    LineNumber = Math.Round(Random.Range(LineNumberMin, LineNumberMax),1);
                    valid = true;
                }
            }
            Formula = $"{LineConstant.ToString("G", CultureInfo.InvariantCulture)}*x+({LineNumber.ToString("G", CultureInfo.InvariantCulture)})";
        }
        else if (FormulaType == 5)
        {
            bool valid = false;
            double ConstantOne = 0;
            double ConstantThree = 0;
            int Degree = 0;
            while (!valid)
            {
                // Randomize parameters
                ConstantOne = Math.Round(Random.Range(10f, 30f), 1);
                ConstantThree = Math.Round(Random.Range(10f, 100f), 1);
                Degree = Random.Range(0, 2) == 0 ? 2 : 3;

                float yNeg10 = Mathf.Pow((float)ConstantOne + (-10), Degree) / (float)ConstantThree;
                float y10 = Mathf.Pow((float)ConstantOne + 10, Degree) / (float)ConstantThree;

                // Check for finite results and range
                if (IsValidY(yNeg10) && IsValidY(y10))
                {
                    valid = true;
                }
            }

            Formula = $"({ConstantOne.ToString("G", CultureInfo.InvariantCulture)}+x)^{Degree.ToString("G", CultureInfo.InvariantCulture)}/{ConstantThree.ToString("G", CultureInfo.InvariantCulture)}";

        }
        else if (FormulaType == 6)
        {
            bool valid = false;
            double ConstantOne = 0;
            double ConstantTwo = 0;
            while (!valid)
            {
                ConstantOne = Math.Round(Random.Range(-4f, 4f), 1); // Safe range for slope

                float absG = Mathf.Abs((float)ConstantOne);
                float Hmin = Mathf.Max(10 * (float)ConstantOne, -10 * (float)ConstantOne);      // Domain: ensure Gx + H ≥ 0
                float Hmax = 81f - 10f * absG;                 // Range: ensure sqrt(Gx + H) ≤ 9

                if (Hmin > Hmax) continue; // No valid H in this configuration

                ConstantTwo = Math.Round(Random.Range(Hmin, Hmax), 1); // Random H within safe range

                float yNeg10 = Mathf.Sqrt((float)ConstantOne * -10 + (float)ConstantTwo);
                float y10 = Mathf.Sqrt((float)ConstantOne * 10 + (float)ConstantTwo);

                if (IsValidY(yNeg10) && IsValidY(y10))
                {
                    valid = true;
                }
            }

            Formula = $"sqrt({ConstantOne.ToString("G", CultureInfo.InvariantCulture)}*x+({ConstantTwo.ToString("G", CultureInfo.InvariantCulture)}))";
        }

        int i1 = 0;
        var obstaclesize = Obstacle.GetComponent<SpriteRenderer>().bounds.size;
        for (float x = -10; x <= 10; x+= 0.5f)
        {
            i1 += 1;
            string Result = Formula.Replace("x", x.ToString("G", CultureInfo.InvariantCulture));
            ExpressionEvaluator.Evaluate(Result, out float y1);

            int i2 = 0;
            for (float y2 = -9; y2 <= 9f; y2 += 0.5f)
            {
                i2 += 1;
                if (y2 >= (y1 + SizeGap) || y2 <= (y1 - SizeGap))
                {
                    
                    var positionOne = new Vector2(x, y2);
                    var obstacleOne = Instantiate(Obstacle, positionOne, Quaternion.identity);
                    obstacleOne.transform.position = positionOne;
                    obstacleOne.name = $"Obstacle {i1}.{i2}";
                }
            }


        }
    }
    private bool IsValidY(float y)
    {
        return y >= -9f && y <= 9f && !float.IsNaN(y) && !float.IsInfinity(y);
    }
    private float SafeEval(float baseVal, float n, float m)
    {
        if (baseVal < 0 && n % 1f != 0f)
            return float.NaN;

        return Mathf.Pow(baseVal, n) / m;
    }



}
