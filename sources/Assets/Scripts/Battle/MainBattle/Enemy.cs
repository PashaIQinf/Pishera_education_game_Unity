using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Массив видов противника игрока.
public enum EnemyType
{
    Enemy1,
    Enemy2,
    Enemy3
}
public class Enemy : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public EnemyType EnemyType;
    [SerializeField] public GameObject _healthBar;
    [SerializeField] public GameObject EnemyObject;
    public float EnemyHealth, EnemyHealthFull;
    public float EnemyDamage;
    public float _updatedHealth;
    public Sprite[] spriteArray;
    public Sprite[] spriteHealthBar;

    private float shakeDuration = 0.1f;
    private float shakeMagnitude = 0.1f;
    private float waitForSeconds;

    private Vector3 originalPosition;
    private TextMeshPro _healthBarText;
    [SerializeField] Save save;
    void Start()
    {
        string saveJson = PlayerPrefs.GetString("Save");
        save = JsonUtility.FromJson<Save>(saveJson);
        // Загрузка врага на основе выбора уровня и вида врага.
        int loadedEnemy = PlayerPrefs.GetInt("LevelEnemy");
        EnemyType = (EnemyType)loadedEnemy;
        spriteRenderer = EnemyObject.GetComponent<SpriteRenderer>();
        _healthBarText = _healthBar.transform.GetChild(0).GetComponent<TextMeshPro>();
        originalPosition = transform.localPosition;
        if (EnemyType == EnemyType.Enemy1)
        {
            EnemyHealth = 100.0f;
            EnemyHealthFull = 100.0f;
            EnemyDamage = 10.0f;
            spriteRenderer.sprite = spriteArray[0];
        }
        else if (EnemyType == EnemyType.Enemy2)
        {
            EnemyHealth = 125.0f;
            EnemyHealthFull = 125.0f;
            EnemyDamage = 15.0f;
            spriteRenderer.sprite = spriteArray[1];
        }
        else if (EnemyType == EnemyType.Enemy3)
        {
            EnemyHealth = 150.0f;
            EnemyHealthFull = 150.0f;
            EnemyDamage = 20.0f;
            spriteRenderer.sprite = spriteArray[2];
        }
        _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[0];
        _healthBarText.text = $"Здоровье: {EnemyHealth}";
        if (save.AnimationSpeed == 0)
        {
            waitForSeconds = 0.09f;
        }
        else if (save.AnimationSpeed == 1)
        {
            waitForSeconds = 0.07f;
        }
        else if (save.AnimationSpeed == 2)
        {
            waitForSeconds = 0.05f;
        }
        else if (save.AnimationSpeed == 3)
        {
            waitForSeconds = 0.03f;
        }
        else if (save.AnimationSpeed == 4)
        {
            waitForSeconds = 0.01f;
        }
    }
    // Функция обновления здоровья врага. 
    public void UpdateHealth(float newHealthValue)
    {
        EnemyHealth = newHealthValue;
        if (EnemyHealth/ EnemyHealthFull == 1)
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[0];
        }
        else if ((EnemyHealth / EnemyHealthFull < 1) && (EnemyHealth / EnemyHealthFull >= 0.95f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[1];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.95f) && (EnemyHealth / EnemyHealthFull >= 0.90f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[2];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.90f) && (EnemyHealth / EnemyHealthFull >= 0.85f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[3];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.85f) && (EnemyHealth / EnemyHealthFull >= 0.80f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[4];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.80f) && (EnemyHealth / EnemyHealthFull >= 0.75f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[5];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.75f) && (EnemyHealth / EnemyHealthFull >= 0.70f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[6];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.70f) && (EnemyHealth / EnemyHealthFull >= 0.65f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[7];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.65f) && (EnemyHealth / EnemyHealthFull >= 0.60f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[8];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.60f) && (EnemyHealth / EnemyHealthFull >= 0.55f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[9];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.55f) && (EnemyHealth / EnemyHealthFull >= 0.50f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[10];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.50f) && (EnemyHealth / EnemyHealthFull >= 0.45f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[11];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.45f) && (EnemyHealth / EnemyHealthFull >= 0.40f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[12];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.40f) && (EnemyHealth / EnemyHealthFull >= 0.35f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[13];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.35f) && (EnemyHealth / EnemyHealthFull >= 0.30f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[14];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.30f) && (EnemyHealth / EnemyHealthFull >= 0.25f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[15];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.25f) && (EnemyHealth / EnemyHealthFull >= 0.20f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[16];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.20f) && (EnemyHealth / EnemyHealthFull >= 0.15f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[17];
        }
        else if ((EnemyHealth / EnemyHealthFull < 0.15f) && (EnemyHealth / EnemyHealthFull > 0))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[18];
        }
        else if (EnemyHealth / EnemyHealthFull == 0)
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[19];
        }
        _healthBarText.text = $"Здоровье: {EnemyHealth}";
    }

    // Функция получения урона на врага.
    public void ReceiveDamage(float damage)
    {
        _updatedHealth = EnemyHealth - damage;
        UpdateHealth(_updatedHealth > 0 ? _updatedHealth : 0);
        if (!save.DisableAnimations)
        {
            PlayShake();
        }
    }
    private void PlayShake()
    {

        StopAllCoroutines(); // останавливает любую текущую анимацию тряски
        StartCoroutine(Shake());
    }

    private IEnumerator Shake()
    {
        float elapsed = 0f;

        while (elapsed < shakeDuration)
        {
            float offsetX = Random.Range(-1f, 1f) * shakeMagnitude;
            float offsetY = Random.Range(-1f, 1f) * shakeMagnitude;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            elapsed += Time.deltaTime;
            yield return new WaitForSeconds(waitForSeconds);
        }

        transform.localPosition = originalPosition;
    }
}
