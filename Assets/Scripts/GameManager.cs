using System;
using System.Collections;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    private void Start()
    {
        StartCoroutine(GameRoutine());
    }


    IEnumerator GameRoutine()
    {
        yield return null;
    }
}
