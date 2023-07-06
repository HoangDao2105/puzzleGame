using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UINextLevel : MonoBehaviour
{
    [SerializeField] private Animation PopUpAni;
    [SerializeField] private Button restartLevelBtn;
    [SerializeField] private Button nextLevelBtn;
    [SerializeField] private Timer countdown;
    private void Awake()
    {
        restartLevelBtn.onClick.AddListener((() =>
        {
            RestartLevel();
        }));
        nextLevelBtn.onClick.AddListener((() =>
        {
            NextLevel();
        }));
    }

    private void OnEnable()
    {
        GameController.OnGamePass += PopUpAnimate;
    }

    private void OnDisable()
    {
        GameController.OnGamePass -= PopUpAnimate;
    }

    void PopUpAnimate()
    {
        PopUpAni.Play("PopUp");
    }

    void HidePanel()
    {
        PopUpAni.Play("Hide");
        countdown.CountDown();
    }

    public void NextLevel()
    {
        StartCoroutine(CR_NextLevelClick());
    }

    public void RestartLevel()
    {
        StartCoroutine(CR_RestartLevelClick());
    }

    IEnumerator CR_NextLevelClick()
    {
        LevelManager.Instance.LevelUp();
        HidePanel();
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.LoadCurLevel();
    }

    IEnumerator CR_RestartLevelClick()
    {
        HidePanel();
        yield return new WaitForSeconds(1f);
        LevelManager.Instance.RestartLevel();
    }
}
