using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip pop;
    
    [SerializeField] private AudioSource effect;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void Start()
    {
        effect = GetComponent<AudioSource>();
    }

    public void PlaySound(string sound)
    {
        switch (sound)
        {
            case "Pop":
                effect.PlayOneShot(pop);
                break;
            case "Click":
                effect.PlayOneShot(click);
                break;
            default:
                break;
        }
    }
}
