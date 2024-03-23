using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Transform mainMenu, settingsMenu;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private Slider musicBackSlider, fxSlider;


    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
            musicBackSlider.value= PlayerPrefs.GetFloat("musicVolume");
        }
        else
        {
            musicBackSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        fxSlider.value = 1;
    }

    public void SettingsMenuAc()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(-1200, 0.5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);
    }

    public void SettingsMenuKapat()
    {
        mainMenu.GetComponent<RectTransform>().DOLocalMoveX(0, 0.5f);
        settingsMenu.GetComponent<RectTransform>().DOLocalMoveX(1200, 0.5f);
    }

    public void GameSahnesiGec()
    {
        SceneManager.LoadScene(1);
    }

    public void BackVolumeDegistir()
    {
        musicSource.volume = musicBackSlider.value;
        PlayerPrefs.SetFloat("musicVolume", musicBackSlider.value);
    }

    public void FXVolumeDegistir()
    {
        PlayerPrefs.SetFloat("fxVolume", fxSlider.value);
    }
}
