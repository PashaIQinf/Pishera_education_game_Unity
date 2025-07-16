using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsPauseManager : MonoBehaviour
{
    [SerializeField] private GameObject optionsName;
    [SerializeField] private GameObject optionsSubName;
    [SerializeField] private GameObject soundButton;
    [SerializeField] private GameObject optionsExitButton;
    [SerializeField] private GameObject optionsEscapeButton;

    [SerializeField] private GameObject generalVolumeSlider;
    [SerializeField] private GameObject generalVolumeName;
    [SerializeField] private AudioMixer generalVolumeMixer;

    [SerializeField] private GameObject effectsVolumeSlider;
    [SerializeField] private GameObject effectsVolumeName;

    [SerializeField] private GameObject musicVolumeSlider;
    [SerializeField] private GameObject musicVolumeName;

    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject backgroundMenu;

    private TextMeshProUGUI optionsNameText;
    private TextMeshProUGUI optionsSubNameText;
    private TextMeshProUGUI generalVolumeText;
    private TextMeshProUGUI effectsVolumeText;
    private TextMeshProUGUI musicVolumeText;
    private TextMeshProUGUI optionsEscapeButtonText;
    private RectTransform optionsEscapeButtonRect;


    private const float _multiplier = 20f;
    private int Level;
    private string GeneralVolumeParameter = "GeneralVolume";
    private string EffectsVolumeParameter = "EffectsVolume";
    private string MusicVolumeParameter = "MusicVolume";
    private Slider generalVolumeValue;
    private Slider effectsVolumeValue;
    private Slider musicVolumeValue;
    [SerializeField] Save save;
    private SaveGameData saveGameData = new SaveGameData();

    private void Start()
    {
        Level = PlayerPrefs.GetInt("Level");
        optionsEscapeButtonRect = optionsEscapeButton.GetComponent<RectTransform>();
        optionsEscapeButtonText = optionsEscapeButton.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
        optionsNameText = optionsName.GetComponent<TextMeshProUGUI>();
        optionsSubNameText = optionsSubName.GetComponent<TextMeshProUGUI>();
        generalVolumeText = generalVolumeName.GetComponent<TextMeshProUGUI>();
        effectsVolumeText = effectsVolumeName.GetComponent<TextMeshProUGUI>();
        musicVolumeText = musicVolumeName.GetComponent<TextMeshProUGUI>();

        generalVolumeValue = generalVolumeSlider.GetComponent<Slider>();
        effectsVolumeValue = effectsVolumeSlider.GetComponent<Slider>();
        musicVolumeValue = musicVolumeSlider.GetComponent<Slider>();
        if (Level <= 3)
        {
            optionsNameText.text = "Этап 1";
        }
        else
        {
            optionsNameText.text = "Этап 2";
        }
        optionsSubNameText.text = $"Уровень {Level}";
        save.Level = 0;
        save.GeneralVolume = 100;
        save.EffectsVolume = 100;
        save.MusicVolume = 100;
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

        GeneralVolume();
        EffectsVolume();
        MusicVolume();

    }

    public void SoundButtonClick()
    {
        optionsNameText.text = "Звук";
        soundButton.SetActive(false);
        optionsSubName.SetActive(false);

        generalVolumeSlider.SetActive(true);
        generalVolumeName.SetActive(true);

        effectsVolumeSlider.SetActive(true);
        effectsVolumeName.SetActive(true);

        musicVolumeSlider.SetActive(true);
        musicVolumeName.SetActive(true);
        optionsEscapeButtonRect.localPosition = optionsEscapeButtonRect.localPosition - new Vector3(240, 0f, 0f);
        optionsEscapeButtonText.text = "Назад";
        optionsExitButton.SetActive(false);

    }
    public void ExitButtonClick()
    {
      if (optionsNameText.text == "Звук")
        {
            if (Level <= 3)
            {
                optionsNameText.text = "Этап 1";
            }
            else
            {
                optionsNameText.text = "Этап 2";
            }
            optionsSubName.SetActive(true);
            optionsSubNameText.text = $"Уровень {Level}";
            soundButton.SetActive(true);
            generalVolumeSlider.SetActive(false);
            generalVolumeName.SetActive(false);

            effectsVolumeSlider.SetActive(false);
            effectsVolumeName.SetActive(false);

            musicVolumeSlider.SetActive(false);
            musicVolumeName.SetActive(false);
            optionsEscapeButtonRect.localPosition = optionsEscapeButtonRect.localPosition + new Vector3(240, 0f, 0f);
            optionsEscapeButtonText.text = "Продолжить";
            optionsExitButton.SetActive(true);

            save.GeneralVolume = generalVolumeValue.value;
            save.EffectsVolume = effectsVolumeValue.value;
            save.MusicVolume = musicVolumeValue.value;
            saveGameData.SaveData(save);
        }
        else if(optionsNameText.text != "Звук")
        {
            optionsMenu.SetActive(false);
            backgroundMenu.SetActive(false);
        }
    }
    public void GeneralVolume()
    {
        generalVolumeValue = generalVolumeSlider.GetComponent<Slider>();
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
}

