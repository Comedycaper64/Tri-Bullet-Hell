using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MusicPlayer : MonoBehaviour
{
    public float currentVolume;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private AudioClip battleMusic;
    private AudioSource audioSource;

    private void Awake()
    {
        SetUpSingleton();
        audioSource = GetComponent<AudioSource>();
        currentVolume = audioSource.volume;
    }

    private void SetUpSingleton()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ChangeVolume()
    {
        audioSource.volume = volumeSlider.value;
        currentVolume = volumeSlider.value;
    }

    public void SwitchToMenuMusic()
    {
        GetComponent<AudioSource>().clip = menuMusic;
        GetComponent<AudioSource>().Play();
    }

    public void SwitchToBattleMusic()
    {
        GetComponent<AudioSource>().clip = battleMusic;
        GetComponent<AudioSource>().Play();
    }

}
