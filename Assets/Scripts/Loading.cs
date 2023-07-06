using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    [SerializeField] private Animation anmt;
    [SerializeField] private GameObject loadingUI;
    private void Awake()
    {
        playBtn.onClick.AddListener((() =>
        {
            LoadMainScene();
        }));
    }

    private void Start()
    {
        loadingUI.SetActive(false);
        anmt.Play("Intro");
        
    }
    
    
    void LoadMainScene()
    {
        anmt.Play("Outtro");
        loadingUI.SetActive(true);
        StartCoroutine(CR_LoadMainScene());
    }
    IEnumerator CR_LoadMainScene()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Main");
    }
}
