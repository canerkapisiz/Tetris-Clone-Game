using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    public GameObject[] sayilar;

    public GameObject sayilarTransform;

    GameManager gameManager;

    private void Awake()
    {
        gameManager = GameObject.FindAnyObjectByType<GameManager>();
    }
    
    void Start()
    {
        StartCoroutine(SayilariAc());
    }

    IEnumerator SayilariAc()
    {
        yield return new WaitForSeconds(0.1f);

        sayilarTransform.GetComponent<RectTransform>().DORotate(Vector3.zero, 0.3f).SetEase(Ease.OutBack);
        sayilarTransform.GetComponent<CanvasGroup>().DOFade(1, 0.3f);

        yield return new WaitForSeconds(0.2f);

        int sayac = 0;

        while (sayac < sayilar.Length)
        {
            sayilar[sayac].GetComponent<RectTransform>().DOLocalMoveY(0, 0.5f);
            sayilar[sayac].GetComponent<CanvasGroup>().DOFade(1, 0.5f);
            sayilar[sayac].GetComponent<RectTransform>().DOScale(2f, 0.3f).SetEase(Ease.OutBounce).OnComplete(() =>
            sayilar[sayac].GetComponent<RectTransform>().DOScale(1f, 0.3f).SetEase(Ease.InBack));

            yield return new WaitForSeconds(1.5f);

            sayac++;
            sayilar[sayac-1].GetComponent<RectTransform>().DOLocalMoveY(150f, 0.5f);

            yield return new WaitForSeconds(0.1f);
        }

        sayilarTransform.GetComponent<CanvasGroup>().DOFade(0, 0.5f).OnComplete(() =>
        {
            sayilarTransform.transform.parent.gameObject.SetActive(false);
            gameManager.OyunaBasla();
        });
    }
}
