using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveGameData
{
    public void SaveData(Save save)
    {
        string saveJson = JsonUtility.ToJson(save);
        PlayerPrefs.SetString("Save", saveJson);
        PlayerPrefs.Save();
    }
}

[System.Serializable]
public class Save
{
    public int Level;
    public float GeneralVolume;
    public float EffectsVolume;
    public float MusicVolume;
    public bool DisableEffects;
    public bool DisableAnimations;
    public int AnimationSpeed;
}
