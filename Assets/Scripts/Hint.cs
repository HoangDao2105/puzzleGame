using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Hint : MonoBehaviour
{
    [SerializeField] private int hintCount;
    [SerializeField] private TextMeshProUGUI hintTxt;
    [SerializeField] private Button hintBtn;
    public static event Action OnHintAction;

    private void Awake()
    {
        hintBtn.onClick.AddListener(() =>
        {
            HintAction();
        });
    }

    private void Start()
    {
        hintCount = 2;
        hintTxt.text = "Hint: " + hintCount;
    }

    public void HintAction()
    {
        hintCount--;
        if (hintCount < 0)
        {
            hintBtn.interactable = false;
            hintTxt.text = "Hint: 0";
        }
        else
        {
            hintTxt.text = "Hint: " + hintCount;
            OnHintAction?.Invoke();
        }
    }
}
