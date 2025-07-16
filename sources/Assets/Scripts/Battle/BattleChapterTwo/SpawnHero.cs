using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class SpawnHero : MonoBehaviour
{
    [SerializeField] public GameObject _healthBar;
    public void SpawnPlayer(string Formula, float SizeGap)
    {
        float x = -10f;
        string Result = Formula.Replace("x", x.ToString("G", CultureInfo.InvariantCulture));
        ExpressionEvaluator.Evaluate(Result, out float y2);
        float y1 = ((y2 + SizeGap) + (y2 - SizeGap)) / 2;

        Vector3 SpawnPos = new Vector3(x,y1,0);
        transform.position = SpawnPos;

        Vector3 healthPosition = new Vector3(transform.position.x, transform.position.y + 2, 0);
        _healthBar.transform.position = healthPosition;
    }
    

}
