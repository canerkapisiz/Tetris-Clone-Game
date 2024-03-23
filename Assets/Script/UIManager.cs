using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public bool oyunDurdumu = false;
    public GameObject pausePanel;
    private GameManager gameManager;

    void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        if (pausePanel)
        {
            pausePanel.SetActive(false);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PausePanelAcKapat();
        }
    }

    public void PausePanelAcKapat()
    {
        if (gameManager.gameOver)
        {
            return;
        }

        if (pausePanel)
        {
            pausePanel.SetActive(oyunDurdumu);
            if (SoundManager.instance)
            {
                SoundManager.instance.SesEfektiCikar(0);
                Time.timeScale = (oyunDurdumu) ? 1 : 0;
            }
        }
    }

    public void YenidenOyna()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
