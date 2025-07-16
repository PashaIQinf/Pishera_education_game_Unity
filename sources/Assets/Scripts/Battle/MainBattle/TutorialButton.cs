using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialButton : MonoBehaviour
{
    [SerializeField] private GameObject tutorialPrefab;
    private TutorialManager tutorialManager;
    void Start()
    {
        tutorialManager = tutorialPrefab.transform.GetChild(0).GetComponent<TutorialManager>();
        tutorialManager.IsReference = false;
    }
    public void OnButtonClick()
    {
        tutorialManager.IsReference = true;
        Instantiate(tutorialPrefab);
    }
}
