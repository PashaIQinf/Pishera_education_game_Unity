using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioButton : MonoBehaviour
{
    [SerializeField] private AudioSource buttonSource;
    [SerializeField] private AudioClip audioClick;

    public void OnButtonClick()
    {
        buttonSource.PlayOneShot(audioClick);
    }

}
