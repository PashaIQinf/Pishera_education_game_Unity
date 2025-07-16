using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Скрипт для движения героя по математической функции, заданной как строка.
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class PathFunctionFollower2D : MonoBehaviour
{
    [Header("Функция движения")]
    [Tooltip("Функция от x, например: sin(x)*2")]
    public string functionString = "";

    [Tooltip("Начало диапазона X")]
    public float xStart = -10f;

    [Tooltip("Конец диапазона X")]
    public float xEnd = 10f;

    [Tooltip("Ограничение длины пути")]
    public float maxPathLength = 10f;

    [Tooltip("Шаг по X (чем меньше, тем плавнее)")]
    public float resolution = 0.1f;

    [Tooltip("Скорость движения вдоль траектории")]
    public float moveSpeed = 2f;


    [Tooltip("Показать линию траектории")]
    public bool showPath = true;

    [Header("Настройки столкновений")]
    [Tooltip("Сила отдачи при столкновении")]
    public float recoilForce = 5f;


    private List<Vector3> pathPoints = new List<Vector3>();
    private int currentPointIndex = 0;
    private bool isMoving = false;
    private bool isShowing = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform heroPosition;
    [SerializeField] private Hero _hero;
    [SerializeField] public GameObject _healthBar;
    [SerializeField] private AudioSource heroSource;
    [SerializeField] private AudioClip audioCrash;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        lineRenderer = GetComponent<LineRenderer>();

    }

    void FixedUpdate()
    {
        if (!isMoving)
        {
            rb.AddForce(Vector2.up * 20f, ForceMode2D.Impulse);
            rb.velocity = Vector2.zero;
            return;
        }
        if (pathPoints.Count == 0 || currentPointIndex >= pathPoints.Count)
        {
            rb.velocity = Vector2.zero;
            StopMovement();
            return;
        }

        // Только рассчитываем путь заранее, но НЕ начинаем движение

        Vector3 target = pathPoints[currentPointIndex];
        Vector3 direction = (target - transform.position).normalized;

        rb.velocity = direction * moveSpeed;


        if (Vector2.Distance(transform.position, target) < 0.1f)
        {
            currentPointIndex++;
        }

        RotateTowardsDirection(direction);

        if (heroPosition.position.x > 10)
        {
            StopMovement();

        }
    }

    private void RotateTowardsDirection(Vector2 direction)
    {
        if (direction == Vector2.zero) return;

        // Получаем угол направления движения
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Вращаем только по Z
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);

        // Плавный поворот (можно менять скорость поворота)
        float turnSpeed = 5f;
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * turnSpeed);
    }

    private void GeneratePath()
    {
        pathPoints.Clear();

        Vector3 startPos = transform.position; // позиция объекта

        // Вычисляем f(xStart), чтобы потом вычесть из всех значений Y
        string y0String = functionString.Replace("x", xStart.ToString("G", System.Globalization.CultureInfo.InvariantCulture));
        ExpressionEvaluator.Evaluate(y0String, out float yStart);

        float totalLength = 0f;
        Vector3? previousPoint = null;

        for (float x = xStart; x <= xEnd; x += resolution)
        {
            string Result = functionString.Replace("x", x.ToString("G", System.Globalization.CultureInfo.InvariantCulture));
            ExpressionEvaluator.Evaluate(Result, out float y);


            // Смещаем X и Y так, чтобы (xStart, f(xStart)) = позиция объекта
            float localX = x - xStart;
            float localY = y - yStart;

            float worldX = startPos.x + localX;     // Привязка X
            float worldY = startPos.y + localY;     // Привязка Y
            
            Vector3 newPoint = new Vector3(worldX, worldY, 0);

            if (previousPoint.HasValue)
            {
                float segmentLength = Vector3.Distance(previousPoint.Value, newPoint);

                if (totalLength + segmentLength > maxPathLength)
                {
                    // Добавим частичную точку до нужной длины
                    float remaining = maxPathLength - totalLength;
                    Vector3 direction = (newPoint - previousPoint.Value).normalized;
                    Vector3 finalPoint = previousPoint.Value + direction * remaining;
                    pathPoints.Add(finalPoint);
                    break;
                }

                totalLength += segmentLength;
            }

            pathPoints.Add(newPoint);
            previousPoint = newPoint;

        }
 }

    private void DrawPath()
    {
        lineRenderer.positionCount = pathPoints.Count;
        lineRenderer.SetPositions(pathPoints.ToArray());
        lineRenderer.enabled = true;
    }

    public void StartMovement()
    {
        currentPointIndex = 0;
        isMoving = true;
        _healthBar.SetActive(false);
    }

    public void ShowMovement()
    {
        GeneratePath();
        DrawPath();
        isShowing = true;
    }

    public void HideMovement()
    {
        if (isShowing == true)
        {
            lineRenderer.enabled = false;
            isShowing = false;
        }
    }

    public void StopMovement()
    {
        isMoving = false;
        rb.velocity = Vector2.zero;

        lineRenderer.positionCount = 0;
        lineRenderer.SetPositions(new Vector3[0]);
        lineRenderer.enabled = false;

        pathPoints.Clear();
    }

    // Реакция на столкновения с другими объектами
    private void OnCollisionEnter2D(Collision2D collision)
    {
        _hero.ReceiveDamage(10f);
        _healthBar.SetActive(true);
        Vector2 healthPosition = new Vector2(heroPosition.position.x, heroPosition.position.y + 2);
        _healthBar.transform.position = healthPosition;
        heroSource.PlayOneShot(audioCrash);
        Vector2 collisionNormal = collision.contacts[0].normal; // Нормаль от столкновения
        rb.AddForce(collisionNormal * this.recoilForce, ForceMode2D.Impulse);

        StartCoroutine(DelayedStopMovement(0.2f)); // Автоматическая остановка при столкновении


    }
    private IEnumerator DelayedStopMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        StopMovement();
    }


}
