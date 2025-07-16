using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsedCardsRepository : MonoBehaviour
{
    // Инициализация массивов для обозначение выложенных карт и карт для перестановки.
    public List<GameObject> UsedCards = new List<GameObject>();
    public List<GameObject> ReplaceCards = new List<GameObject>();
}
