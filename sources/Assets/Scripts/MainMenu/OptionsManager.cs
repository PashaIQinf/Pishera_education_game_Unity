using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsName;
    [SerializeField] private GameObject graphicsButton;
    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject optionsExitButton;

    [SerializeField] private GameObject generalVolumeSlider;
    [SerializeField] private GameObject generalVolumeName;
    [SerializeField] private AudioMixer generalVolumeMixer;

    [SerializeField] private GameObject effectsVolumeSlider;
    [SerializeField] private GameObject effectsVolumeName;

    [SerializeField] private GameObject musicVolumeSlider;
    [SerializeField] private GameObject musicVolumeName;

    [SerializeField] private GameObject disableEffectsToggle;
    [SerializeField] private GameObject disableAnimationsToggle;

    [SerializeField] private GameObject animationSpeedSlider;
    [SerializeField] private GameObject animationSpeedName;

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject mainMenu;

    private TextMeshProUGUI optionsNameText;
    private TextMeshProUGUI generalVolumeText;
    private TextMeshProUGUI effectsVolumeText;
    private TextMeshProUGUI musicVolumeText;
    private TextMeshProUGUI animationSpeedText;

    private const float _multiplier = 20f;
    private string GeneralVolumeParameter = "GeneralVolume";
    private string EffectsVolumeParameter = "EffectsVolume";
    private string MusicVolumeParameter = "MusicVolume";
    private Slider generalVolumeValue;
    private Slider effectsVolumeValue;
    private Slider musicVolumeValue;
    private Slider animationSpeedValue;
    private Toggle effectsToggle;
    private Toggle animationsToggle;

    [SerializeField] Save save;
    private SaveGameData saveGameData = new SaveGameData();
    private void Start()
    {
        optionsNameText = optionsName.GetComponent<TextMeshProUGUI>();
        generalVolumeText = generalVolumeName.GetComponent<TextMeshProUGUI>();
        effectsVolumeText = effectsVolumeName.GetComponent<TextMeshProUGUI>();
        musicVolumeText = musicVolumeName.GetComponent<TextMeshProUGUI>();
        animationSpeedText = animationSpeedName.GetComponent<TextMeshProUGUI>();

        generalVolumeValue = generalVolumeSlider.GetComponent<Slider>();
        effectsVolumeValue = effectsVolumeSlider.GetComponent<Slider>();
        musicVolumeValue = musicVolumeSlider.GetComponent<Slider>();
        animationSpeedValue = animationSpeedSlider.GetComponent<Slider>();


        effectsToggle = disableEffectsToggle.GetComponent<Toggle>();
        animationsToggle = disableAnimationsToggle.GetComponent<Toggle>();

        save.Level = 0;
        save.GeneralVolume = 100;
        save.EffectsVolume = 100;
        save.MusicVolume = 100;
        save.DisableEffects = false;
        save.DisableAnimations = false;
        save.AnimationSpeed = 2;
        if (PlayerPrefs.HasKey("Save"))
        {
            string saveJson = PlayerPrefs.GetString("Save");
            save = JsonUtility.FromJson<Save>(saveJson);
        }
        else
        {
            saveGameData.SaveData(save);
        }
        generalVolumeValue.value = save.GeneralVolume;
        effectsVolumeValue.value = save.EffectsVolume;
        musicVolumeValue.value = save.MusicVolume;
        effectsToggle.isOn = save.DisableEffects;
        animationsToggle.isOn = save.DisableAnimations;
        animationSpeedValue.value = save.AnimationSpeed;

        GeneralVolume();
        EffectsVolume();
        MusicVolume();
        AnimationSpeed();

    }

    public void GraphicsButtonClick()
    {
        optionsNameText.text = "Графика";
        graphicsButton.SetActive(false);
        soundButton.SetActive(false);

        animationSpeedSlider.SetActive(true);
        animationSpeedName.SetActive(true);

        disableEffectsToggle.SetActive(true);
        disableAnimationsToggle.SetActive(true);
    }

    public void SoundButtonClick()
    {
        optionsNameText.text = "Звук";
        graphicsButton.SetActive(false);
        soundButton.SetActive(false);

        generalVolumeSlider.SetActive(true);
        generalVolumeName.SetActive(true);

        effectsVolumeSlider.SetActive(true);
        effectsVolumeName.SetActive(true);

        musicVolumeSlider.SetActive(true);
        musicVolumeName.SetActive(true);
    }

    public void ExitButtonClick()
    {
      if (optionsNameText.text == "Звук")
        {
            optionsNameText.text = "Настройки";
            graphicsButton.SetActive(true);
            soundButton.SetActive(true);

            generalVolumeSlider.SetActive(false);
            generalVolumeName.SetActive(false);

            effectsVolumeSlider.SetActive(false);
            effectsVolumeName.SetActive(false);

            musicVolumeSlider.SetActive(false);
            musicVolumeName.SetActive(false);

            save.GeneralVolume = generalVolumeValue.value;
            save.EffectsVolume = effectsVolumeValue.value;
            save.MusicVolume = musicVolumeValue.value;
            saveGameData.SaveData(save);
        }
      else if (optionsNameText.text == "Графика")
        {
            optionsNameText.text = "Настройки";
            graphicsButton.SetActive(true);
            soundButton.SetActive(true);

            animationSpeedSlider.SetActive(false);
            animationSpeedName.SetActive(false);

            disableEffectsToggle.SetActive(false);
            disableAnimationsToggle.SetActive(false);

            save.DisableEffects = effectsToggle.isOn;
            save.DisableAnimations = animationsToggle.isOn;
            save.AnimationSpeed = (int)animationSpeedValue.value;
            saveGameData.SaveData(save);
        }
        else if(optionsNameText.text == "Настройки")
        {
            optionsMenu.SetActive(false);
            mainMenu.SetActive(true);
        }
    }
    public void GeneralVolume()
    {
        generalVolumeText.text = $"Общая громкость: {generalVolumeValue.value}";
        float generalVolume = 0f;
        if (generalVolumeValue.value == 0)
        {
            generalVolume = Mathf.Log10((generalVolumeValue.value + 0.00001f) / 100) * _multiplier;
        }
        else
        {
            generalVolume = Mathf.Log10(generalVolumeValue.value / 100) * _multiplier;
        }
        generalVolumeMixer.SetFloat(GeneralVolumeParameter, generalVolume);

    }

    public void EffectsVolume()
    {
        effectsVolumeText.text = $"Громкость эффектов: {effectsVolumeValue.value}";
        float effectsVolume = 0f;
        if (effectsVolumeValue.value == 0)
        {
            effectsVolume = Mathf.Log10((effectsVolumeValue.value + 0.00001f) / 100) * _multiplier;
        }
        else
        {
            effectsVolume = Mathf.Log10(effectsVolumeValue.value / 100) * _multiplier;
        }
        generalVolumeMixer.SetFloat(EffectsVolumeParameter, effectsVolume);
    }

    public void MusicVolume()
    {
        musicVolumeText.text = $"Громкость музыки: {musicVolumeValue.value}";
        float musicVolume = 0f;
        if (musicVolumeValue.value == 0)
        {
            musicVolume = Mathf.Log10((musicVolumeValue.value + 0.00001f) / 100) * _multiplier;
        }
        else
        {
            musicVolume = Mathf.Log10(musicVolumeValue.value / 100) * _multiplier;
        }
        generalVolumeMixer.SetFloat(MusicVolumeParameter, musicVolume);
    }
    public void AnimationSpeed()
    {
        if (animationSpeedValue.value == 0)
        {
            animationSpeedText.text = $"Скорость анимаций: Очень медленная";;
        }
        else if (animationSpeedValue.value == 1)
        {
            animationSpeedText.text = $"Скорость анимаций: Медленная";
        }
        else if (animationSpeedValue.value == 2)
        {
            animationSpeedText.text = $"Скорость анимаций: Нормальная";
        }
        else if (animationSpeedValue.value == 3)
        {
            animationSpeedText.text = $"Скорость анимаций: Быстрая";
        }
        else if (animationSpeedValue.value == 4)
        {
            animationSpeedText.text = $"Скорость анимаций: Очень быстрая";
        }
    }
}

