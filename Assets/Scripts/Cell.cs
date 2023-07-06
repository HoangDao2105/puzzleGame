using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cell : MonoBehaviour
{
    [SerializeField] private Image contentImage;
    [SerializeField] private Button contentButton;
    [SerializeField] private GameObject cellImage;
    
    public Image Image
    {
        get { return contentImage; }
    }
    // Start is called before the first frame update
    void Start()
    {
        contentButton = GetComponent<Button>();
        contentImage = GetComponent<Image>();
        
    }

    public void SelectingCell()
    {
        cellImage.SetActive(false);
        contentButton.enabled = false;
    }

    public void DeSelectedCell()
    {
        cellImage.SetActive(true);
        contentButton.enabled = true;
    }

    public void Hint()
    {
        cellImage.SetActive(false);
    }
}
