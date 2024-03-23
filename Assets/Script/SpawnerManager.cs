using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SpawnerManager : MonoBehaviour
{
    [SerializeField] private ShapeManager[] tumSekiller;
    [SerializeField] private Image[] sekilImages = new Image[2];
    ShapeManager[] siradakiSekiller = new ShapeManager[2];

    public ShapeManager SekilOlustur()
    {
        ShapeManager sekil = null;

        sekil = SiradakiSekliAl();
        sekil.gameObject.SetActive(true);
        sekil.transform.position = transform.position;

        if (sekil != null)
        {
            return sekil;
        }
        else
        {
            return null;
        }
    }

    ShapeManager RastgeleSekilOlustur()
    {
        int randomSekil = Random.Range(0, tumSekiller.Length);

        if (tumSekiller[randomSekil])
        {
            return tumSekiller[randomSekil];
        }
        else
        {
            return null;
        }
    }

    public void HepsiniNullYap()
    {
        for (int i = 0; i < siradakiSekiller.Length; i++)
        {
            siradakiSekiller[i] = null;
        }
        SirayiDoldur();
    }

    void SirayiDoldur()
    {
        for (int i = 0; i < siradakiSekiller.Length; i++)
        {
            if (!siradakiSekiller[i])
            {
                siradakiSekiller[i] = Instantiate(RastgeleSekilOlustur(), transform.position, Quaternion.identity) as ShapeManager;
                siradakiSekiller[i].gameObject.SetActive(false);
                sekilImages[i].sprite = siradakiSekiller[i].shapeSekil;
            }
        }

        StartCoroutine(SekilImageAc());
    }

    IEnumerator SekilImageAc()
    {
        for (int i = 0; i < sekilImages.Length; i++)
        {
            sekilImages[i].GetComponent<CanvasGroup>().alpha = 0f;
            sekilImages[i].GetComponent<RectTransform>().localScale = Vector3.zero;
        }

        yield return new WaitForSeconds(0.1f);

        int sayac = 0;

        while (sayac < sekilImages.Length)
        {
            sekilImages[sayac].GetComponent<CanvasGroup>().DOFade(1, 0.6f);
            sekilImages[sayac].GetComponent<RectTransform>().DOScale(1, 0.6f).SetEase(Ease.OutBack);
            sayac++;

            yield return new WaitForSeconds(0.4f);
        }
    }

    ShapeManager SiradakiSekliAl()
    {
        ShapeManager sonrakiSekil = null;

        if (siradakiSekiller[0])
        {
            sonrakiSekil = siradakiSekiller[0];
        }

        for (int i = 1; i < siradakiSekiller.Length; i++)
        {
            siradakiSekiller[i - 1] = siradakiSekiller[i];
            sekilImages[i - 1].sprite = siradakiSekiller[i - 1].shapeSekil;
        }

        siradakiSekiller[siradakiSekiller.Length - 1] = null;

        SirayiDoldur();

        return sonrakiSekil;
    }

    public ShapeManager EldekiShapeOlustur()
    {
        ShapeManager eldekiSekil = null;

        eldekiSekil = Instantiate(RastgeleSekilOlustur(), transform.position, Quaternion.identity) as ShapeManager;
        eldekiSekil.transform.position = transform.position;
        return eldekiSekil;
    }
}
