using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    [SerializeField] private Image frige;
    

    private void OnEnable()
    {
        GameController.OnGetScore += OnGetScore;
    }

    private void OnDisable()
    {
        GameController.OnGetScore -= OnGetScore;
    }

    public void OnGetScore()
    {
        StartCoroutine(CR_OnGetScore());
    }

    IEnumerator CR_OnGetScore()
    {
        yield return new WaitForSeconds(0.6f);
        LeanTween.cancel(frige.gameObject);
        LeanTween.scaleX(frige.gameObject, 2.0f, 0.5f).setEasePunch();
        LeanTween.scaleY(frige.gameObject, 2.0f, 0.5f).setEasePunch();
    }
}
