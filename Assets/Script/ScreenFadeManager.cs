using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScreenFadeManager : MonoBehaviour
{
    public float baslangicAlpha = 1f;
    public float bitisAlpha = 0f;

    public float beklemeSuresi = 0f;
    public float fadeSuresi = 1f;

    void Start()
    {
        GetComponent<CanvasGroup>().alpha = baslangicAlpha;

        StartCoroutine(FadeSuresi());
    }

    IEnumerator FadeSuresi()
    {
        yield return new WaitForSeconds(beklemeSuresi);

        GetComponent<CanvasGroup>().DOFade(bitisAlpha, fadeSuresi);
    }
}
