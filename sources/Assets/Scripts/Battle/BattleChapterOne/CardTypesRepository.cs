using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardTypesRepository : MonoBehaviour
{
    // Инициализация типов и видов карт на уровнях 1,2,3.
    public List<string> ValueCardsOne = new List<string>(new string[] { "()^x", "n", "+", "i" });
    public List<string> ComparisonOperations = new List<string>(new string[] { ">","<" });
    public List<string> Operations = new List<string>(new string[] { "+", "-","*", "/" });
}
