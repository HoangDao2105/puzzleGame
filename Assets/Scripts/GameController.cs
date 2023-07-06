using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;


public class GameController : MonoBehaviour
{

    [SerializeField] private List<Button> Buttons; // Array of buttons
    [SerializeField] private Image selectedImage; // Selected current Image
    [SerializeField] private Cell selectedCell;
    [SerializeField] private GameObject cloneImage;
    [SerializeField] private GameObject ps;
    public static event Action OnGamePass;
    public static event Action OnGetScore;

    private void OnEnable()
    {
        Hint.OnHintAction += HintGame;
    }

    private void OnDisable()
    {
        Hint.OnHintAction -= HintGame;
    }

    private void Awake()
    {
        Buttons = GetComponentsInChildren<Button>().ToList();
        Debug.Log("Get all button completed");
        for (int i = 0; i < Buttons.Count; i++)
        {
            Cell buttonImage = Buttons[i].GetComponent<Cell>();
            Buttons[i].onClick.AddListener((() => SelectCell(buttonImage)));
        }
    }

    public void SelectCell(Cell imageCell)
    {
        if (selectedImage == null)
        {
            // If selectedImage is null 
            selectedCell = imageCell;
            selectedImage = imageCell.Image;
            imageCell.SelectingCell();
            //Play audio clip
            SoundManager.Instance.PlaySound("Click");
        }
        else
        {
            if (selectedImage.sprite == imageCell.Image.sprite)
            {
                // Checking
                Debug.Log("Match!");
                Buttons.Remove(imageCell.GetComponent<Button>());
                Buttons.Remove(selectedCell.GetComponent<Button>());
                if (Buttons.Count <= 0)
                {
                    Debug.Log("GamePassed");
                    OnGamePass?.Invoke();
                }
                //When player get score play lean tween
                GameObject imageClone = Instantiate(cloneImage, 
                    imageCell.transform.position, quaternion.identity,transform);
                imageClone.GetComponent<Image>().sprite = imageCell.Image.sprite;
                LeanTween.cancel(imageClone.gameObject);
                LeanTween.move(imageClone.GetComponent<RectTransform>(), new Vector3(0f, -621f, 0f), 0.5f);
                //Remove cells were found
                Destroy(imageClone.gameObject, 0.7f);
                Destroy(imageCell.gameObject);
                Destroy(selectedCell.gameObject);
                //Play lEAN TWEEN  UI
                OnGetScore?.Invoke();
                //Play audio clip
                SoundManager.Instance.PlaySound("Pop");
            }
            else
            {
                Debug.Log("Not match!");
                imageCell.SelectingCell();
                StartCoroutine(CR_WrongSelect(imageCell, selectedCell));
                //Play audio clip
                SoundManager.Instance.PlaySound("Click");
            }
            //Reset to gameplay
            selectedImage = null;
            selectedCell = null;
        }
    }

    public void HintGame()
    {
        int rand = Random.Range(0, Buttons.Count);
        int index = 0;
        for (int i = 0; i < Buttons.Count; i++)
        {
            if (i!=rand && Buttons[i].image.sprite == Buttons[rand].image.sprite)
            {
                index = i;
                break;
            }
        }
        //Play particle system
        GameObject effect = Instantiate(ps, Buttons[index].transform.position, quaternion.identity, transform);
        Buttons[index].GetComponent<Cell>().Hint();
        Buttons[rand].GetComponent<Cell>().Hint();
        Destroy(effect,0.25f);
    }

    IEnumerator CR_WrongSelect(Cell cell1, Cell cell2)
    {
        yield return new WaitForSeconds(0.5f);
        cell1.DeSelectedCell();
        cell2.DeSelectedCell();
    }
}
