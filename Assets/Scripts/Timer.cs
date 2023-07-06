using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float timeLeft;
    [SerializeField] private TextMeshProUGUI timertxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    private void Start()
    {
        CountDown();
    }


    public void CountDown()
    {
        StopAllCoroutines();
        levelTxt.text = "level " + LevelManager.Instance.CurLevelIndex;
        timeLeft = 60;
        StartCoroutine(CR_Timer());
    }
    IEnumerator CR_Timer()
    {
        while (timeLeft>0)
        {
            yield return new WaitForSeconds(1f);
            timeLeft--;
            timertxt.text = "00:"+timeLeft;
        }
        timertxt.text = "Time out :(";
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.RestartLevel();
        timeLeft = 60;
        StartCoroutine(CR_Timer());
    }
}
