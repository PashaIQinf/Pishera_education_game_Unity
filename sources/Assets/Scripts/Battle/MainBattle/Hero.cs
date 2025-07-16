using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Hero : MonoBehaviour
{
    [SerializeField] public GameObject _healthBar;
    public float HeroHealth, HeroHealthFull;
    public float _updatedHealth;
    public Sprite[] spriteHealthBar;

    private float shakeDuration = 0.1f;
    private float shakeMagnitude = 0.1f;
    private float waitForSeconds;

    private Vector3 originalPosition;
    private TextMeshPro _healthBarText;
    private int level = 0;

    [SerializeField] Save save;
    // Инициализация здоровья игрока.
    void Start()
    {
        string saveJson = PlayerPrefs.GetString("Save");
        level = PlayerPrefs.GetInt("Level");
        save = JsonUtility.FromJson<Save>(saveJson);
        HeroHealth = 100.0f;
        HeroHealthFull = 100.0f;
        _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[0];
        _healthBarText = _healthBar.transform.GetChild(0).GetComponent<TextMeshPro>();
        _healthBarText.text = $"Здоровье: {HeroHealth}";
        originalPosition = transform.localPosition;
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
    // Функция обновления здоровья игрока. 
    public void UpdateHealth(float newHealthValue)
    {
        HeroHealth = newHealthValue;
        if (HeroHealth / HeroHealthFull == 1)
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[0];
        }
        else if ((HeroHealth / HeroHealthFull < 1) && (HeroHealth / HeroHealthFull >= 0.95f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[1];
        }
        else if ((HeroHealth / HeroHealthFull < 0.95f) && (HeroHealth / HeroHealthFull >= 0.90f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[2];
        }
        else if ((HeroHealth / HeroHealthFull < 0.90f) && (HeroHealth / HeroHealthFull >= 0.85f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[3];
        }
        else if ((HeroHealth / HeroHealthFull < 0.85f) && (HeroHealth / HeroHealthFull >= 0.80f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[4];
        }
        else if ((HeroHealth / HeroHealthFull < 0.80f) && (HeroHealth / HeroHealthFull >= 0.75f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[5];
        }
        else if ((HeroHealth / HeroHealthFull < 0.75f) && (HeroHealth / HeroHealthFull >= 0.70f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[6];
        }
        else if ((HeroHealth / HeroHealthFull < 0.70f) && (HeroHealth / HeroHealthFull >= 0.65f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[7];
        }
        else if ((HeroHealth / HeroHealthFull < 0.65f) && (HeroHealth / HeroHealthFull >= 0.60f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[8];
        }
        else if ((HeroHealth / HeroHealthFull < 0.60f) && (HeroHealth / HeroHealthFull >= 0.55f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[9];
        }
        else if ((HeroHealth / HeroHealthFull < 0.55f) && (HeroHealth / HeroHealthFull >= 0.50f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[10];
        }
        else if ((HeroHealth / HeroHealthFull < 0.50f) && (HeroHealth / HeroHealthFull >= 0.45f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[11];
        }
        else if ((HeroHealth / HeroHealthFull < 0.45f) && (HeroHealth / HeroHealthFull >= 0.40f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[12];
        }
        else if ((HeroHealth / HeroHealthFull < 0.40f) && (HeroHealth / HeroHealthFull >= 0.35f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[13];
        }
        else if ((HeroHealth / HeroHealthFull < 0.35f) && (HeroHealth / HeroHealthFull >= 0.30f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[14];
        }
        else if ((HeroHealth / HeroHealthFull < 0.30f) && (HeroHealth / HeroHealthFull >= 0.25f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[15];
        }
        else if ((HeroHealth / HeroHealthFull < 0.25f) && (HeroHealth / HeroHealthFull >= 0.20f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[16];
        }
        else if ((HeroHealth / HeroHealthFull < 0.20f) && (HeroHealth / HeroHealthFull >= 0.15f))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[17];
        }
        else if ((HeroHealth / HeroHealthFull < 0.15f) && (HeroHealth / HeroHealthFull > 0))
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[18];
        }
        else if (HeroHealth / HeroHealthFull == 0)
        {
            _healthBar.GetComponent<SpriteRenderer>().sprite = spriteHealthBar[19];
        }
        _healthBarText.text = $"Здоровье: {HeroHealth}";
    }

    // Функция получения урона на игрока.
    public void ReceiveDamage(float damage)
    {
        _updatedHealth = HeroHealth - damage;
        UpdateHealth(_updatedHealth > 0 ? _updatedHealth : 0);
        if (!save.DisableAnimations && level <= 3)
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
