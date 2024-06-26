using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSettıngManager : MonoBehaviour
{
    GameManager gameManager;
    TouchManager touchManager;

    public Slider dokunmaSlider, suruklemeSlider, dokunmaHizSlider;

    void Start()
    {
        gameManager = GameObject.FindObjectOfType<GameManager>();
        touchManager = GameObject.FindObjectOfType<TouchManager>();

        if(dokunmaSlider != null)
        {
            dokunmaSlider.value = 100;
            dokunmaSlider.minValue = 50;
            dokunmaSlider.maxValue = 150;
        }

        if(suruklemeSlider != null)
        {
            suruklemeSlider.value = 50;
            suruklemeSlider.minValue = 20;
            suruklemeSlider.maxValue = 250;
        }

        if(dokunmaHizSlider != null)
        {
            dokunmaHizSlider.value = 0.15f;
            dokunmaHizSlider.minValue = 0.05f;
            dokunmaHizSlider.maxValue = 0.5f;
        }
    }

    void Update()
    {
        
    }

    public void UpdateSettingsPanel()
    {
        if(dokunmaSlider != null && touchManager != null)
        {
            touchManager.minDragUzaklik = (int)dokunmaSlider.value;
        }

        if (suruklemeSlider != null && touchManager != null)
        {
            touchManager.minSuruklemeUzaklik = (int)suruklemeSlider.value;
        }

        if (dokunmaHizSlider != null && gameManager != null)
        {
            gameManager.minDokunmaZamani = (int)dokunmaHizSlider.value;
        }
    }
}
