using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class GameManager : MonoBehaviour
{
    SpawnerManager spawner;
    BoardManager board;

    ShapeManager aktifSekil;

    [Header("Sayaclar")]
    [Range(0.01f, 1f)]
    [SerializeField] private float asagiInmeSuresi = 0.5f;
    float asagiInmeSayac;
    float asagiInmeLevelSayac;
    [Range(0.01f, 1f)]
    [SerializeField] private float sagSolTusaBasmaSuresi = 0.25f;
    float sagSolTusaBasmaSayac;
    [Range(0.01f, 1f)]
    [SerializeField] private float sagSolDonmeSuresi = 0.25f;
    float sagSolDonmeSayac;
    [Range(0.01f, 1f)]
    [SerializeField] private float asagiTusaBasmaSuresi = 0.25f;
    float asagiTusaBasmaSayac;

    public bool gameOver = false;
    public bool saatYonumu = true;
    public IconAcKapaManager rotateIcon;

    public GameObject gameOverPanel;

    ScoreManager scoreManager;

    TakipShapeManager takipShapeManager;

    ShapeManager eldekiSekil;
    public Image eldekiSekilImage;

    bool eldekiDegistirilebilirmi = true;

    bool hareketEtsinmi = true;

    public ParticleManager[] seviyeAtlamaEfektleri = new ParticleManager[5];
    public ParticleManager[] gameOverEfektleri = new ParticleManager[5];

    enum Direction { none,sol,sag,yukari,asagi}

    Direction suruklemeYonu = Direction.none;
    Direction suruklemeBitisYonu = Direction.none;

    float sonrakiDokunmaZamani, sonrakiSuruklemeZamani;

    [Range(0.05f, 1f)]
    public float minDokunmaZamani = 0.15f;

    [Range(0.05f, 1f)]
    public float minSuruklemeZamani = 0.15f;

    bool dokundumu = false;

    private void Awake()
    {
        board = GameObject.FindAnyObjectByType<BoardManager>();
        spawner = GameObject.FindAnyObjectByType<SpawnerManager>();
        scoreManager = GameObject.FindAnyObjectByType<ScoreManager>();
        takipShapeManager = GameObject.FindAnyObjectByType<TakipShapeManager>();
    }

    private void OnEnable()
    {
        TouchManager.DragEvent += Surukle;
        TouchManager.SwipeEvent += SurukleBitti;
        TouchManager.TapEvent += Tap;
    }

    private void OnDisable()
    {
        TouchManager.DragEvent -= Surukle;
        TouchManager.SwipeEvent -= SurukleBitti;
        TouchManager.TapEvent -= Tap;
    }

    public void OyunaBasla()
    {
        if (spawner)
        {
            spawner.HepsiniNullYap();

            if (aktifSekil == null)
            {
                aktifSekil = spawner.SekilOlustur();
                aktifSekil.transform.position = VectoruIntYap(aktifSekil.transform.position);
            }

            if (aktifSekil)
            {
                aktifSekil.transform.localScale = Vector3.zero;
                hareketEtsinmi = false;
                aktifSekil.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(()=>hareketEtsinmi = true);
            }

            if (eldekiSekil == null)
            {
                eldekiSekil = spawner.EldekiShapeOlustur();

                if (aktifSekil.name == eldekiSekil.name)
                {
                    Destroy(eldekiSekil.gameObject);
                    eldekiSekil = spawner.EldekiShapeOlustur();
                }
                eldekiSekilImage.sprite = eldekiSekil.shapeSekil;
                eldekiSekilImage.GetComponent<CanvasGroup>().alpha = 1f;
                eldekiSekil.gameObject.SetActive(false);
            }
        }

        if (gameOverPanel)
        {
            gameOverPanel.SetActive(false);
        }

        asagiInmeLevelSayac = asagiInmeSuresi;
    }

    void Update()
    {
        if (!board || !spawner || !aktifSekil || gameOver || !scoreManager || !hareketEtsinmi)
        {
            return;
        }

        GirisKontrol();
    }

    void LateUpdate()
    {
        if (!board || !spawner || !aktifSekil || gameOver || !scoreManager || !takipShapeManager || !hareketEtsinmi)
        {
            return;
        }

        if (takipShapeManager)
        {
            takipShapeManager.TakipShapeOlustur(aktifSekil, board);
        }
    }

    void GirisKontrol()
    {
        if ((Input.GetKey("right") && Time.time > sagSolTusaBasmaSayac) || Input.GetKeyDown("right"))
        {
            SagaHareket();
        }
        else if ((Input.GetKey("left") && Time.time > sagSolTusaBasmaSayac) || Input.GetKeyDown("left"))
        {
            SolaHareket();
        }
        else if ((Input.GetKeyDown("up") && Time.time > sagSolDonmeSayac))
        {
            Dondur();
        }
        else if (((Input.GetKey("down") && Time.time > asagiTusaBasmaSayac)) || Time.time > asagiInmeSayac)
        {
            AsagiHareket();
        }
        else if((suruklemeBitisYonu == Direction.sag && Time.time > sonrakiSuruklemeZamani) ||
            (suruklemeYonu == Direction.sag && Time.time>sonrakiDokunmaZamani))
        {
            SagaHareket();
            sonrakiDokunmaZamani = Time.time + minDokunmaZamani;
            sonrakiSuruklemeZamani = Time.time + minSuruklemeZamani;
        }
        else if ((suruklemeBitisYonu == Direction.sol && Time.time > sonrakiSuruklemeZamani) ||
            (suruklemeYonu == Direction.sol) && Time.time>sonrakiDokunmaZamani)
        {
            SolaHareket();
        }
        else if((suruklemeBitisYonu == Direction.yukari && Time.time>sonrakiSuruklemeZamani) || (dokundumu))
        {
            Dondur();
            sonrakiSuruklemeZamani = Time.time + minSuruklemeZamani;
        }
        else if(suruklemeYonu==Direction.asagi && Time.time > sonrakiDokunmaZamani)
        {
            AsagiHareket();
        }

        suruklemeYonu = Direction.none;
        suruklemeBitisYonu = Direction.none;
        dokundumu = false;
    }

    void SagaHareket()
    {
        aktifSekil.SagaHareket();
        sagSolTusaBasmaSayac = Time.time + sagSolTusaBasmaSuresi;
        if (!board.GecerliPozisyondami(aktifSekil))
        {
            SoundManager.instance.SesEfektiCikar(1);
            aktifSekil.SolaHareket();
        }
        else
        {
            SoundManager.instance.SesEfektiCikar(2);
        }
    }

    void SolaHareket()
    {
        aktifSekil.SolaHareket();
        sagSolTusaBasmaSayac = Time.time + sagSolTusaBasmaSuresi;
        if (!board.GecerliPozisyondami(aktifSekil))
        {
            SoundManager.instance.SesEfektiCikar(1);
            aktifSekil.SagaHareket();
        }
        else
        {
            SoundManager.instance.SesEfektiCikar(2);
        }
    }

    void Dondur()
    {
        aktifSekil.SagaDon();
        sagSolTusaBasmaSayac = Time.time + sagSolDonmeSuresi;
        if (!board.GecerliPozisyondami(aktifSekil))
        {
            SoundManager.instance.SesEfektiCikar(1);
            aktifSekil.SolaDon();
        }
        else
        {
            saatYonumu = !saatYonumu;
            if (rotateIcon)
            {
                rotateIcon.IconAcKapat(saatYonumu);
            }
            SoundManager.instance.SesEfektiCikar(2);
        }
    }

    void AsagiHareket()
    {
        asagiTusaBasmaSayac = Time.time + asagiTusaBasmaSuresi;
        asagiInmeSayac = Time.time + asagiInmeLevelSayac;

        if (aktifSekil)
        {
            aktifSekil.AsagiHareket();

            if (!board.GecerliPozisyondami(aktifSekil))
            {
                if (board.DisariTastimi(aktifSekil))
                {
                    aktifSekil.YukariHareket();
                    gameOver = true;
                    SoundManager.instance.SesEfektiCikar(6);

                    if (gameOverPanel)
                    {
                        StartCoroutine(GameOver());
                    }
                    SoundManager.instance.SesEfektiCikar(5);
                }
                else
                {
                    Yerlesti();
                }
            }
        }
    }

    private void Yerlesti()
    {
        if (aktifSekil)
        {
            sagSolTusaBasmaSayac = Time.time;
            asagiTusaBasmaSayac = Time.time;
            sagSolDonmeSayac = Time.time;

            aktifSekil.YukariHareket();
            aktifSekil.YerlesmeEfektleriCikar();
            board.SekliIzgaraIcineAl(aktifSekil);
            SoundManager.instance.SesEfektiCikar(4);

            eldekiDegistirilebilirmi = true;

            if (spawner)
            {
                aktifSekil = spawner.SekilOlustur();

                if (aktifSekil)
                {
                    aktifSekil.transform.localScale = Vector3.zero;
                    hareketEtsinmi = false;
                    aktifSekil.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack).OnComplete(() => hareketEtsinmi = true);
                }

                eldekiSekil = spawner.EldekiShapeOlustur();

                if (aktifSekil.name == eldekiSekil.name)
                {
                    Destroy(eldekiSekil.gameObject);
                    eldekiSekil = spawner.EldekiShapeOlustur();
                    eldekiSekilImage.sprite = eldekiSekil.shapeSekil;
                    eldekiSekil.gameObject.SetActive(false);
                }
                else
                {
                    eldekiSekilImage.sprite = eldekiSekil.shapeSekil;
                    eldekiSekil.gameObject.SetActive(false);
                }
            }

            if (takipShapeManager)
            {
                takipShapeManager.Reset();
            }

            StartCoroutine(board.TumSatirlariTemizle());

            if (board.tamamlananSatir > 0)
            {
                scoreManager.SatirSkoru(board.tamamlananSatir);

                if(scoreManager.levelGecildimi)
                {
                    SoundManager.instance.SesEfektiCikar(7);
                    asagiInmeLevelSayac = asagiInmeSuresi - Mathf.Clamp(((float)scoreManager.level - 1) * 0.1f, 0.05f, 1f);

                    StartCoroutine(SeviyeGec());
                }
                else
                {
                    if (board.tamamlananSatir > 1)
                    {
                        SoundManager.instance.VocalSesiCikar();
                    }
                }
               
                SoundManager.instance.SesEfektiCikar(4);
            }
        }
    }

    Vector2 VectoruIntYap(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

    public void RotationIconYonu()
    {
        saatYonumu = !saatYonumu;
        aktifSekil.SaatYonundeDonsun(saatYonumu);

        if (!board.GecerliPozisyondami(aktifSekil))
        {
            aktifSekil.SaatYonundeDonsun(!saatYonumu);
            SoundManager.instance.SesEfektiCikar(2);
        }
        else
        {
            if (rotateIcon)
            {
                rotateIcon.IconAcKapat(saatYonumu);
            }
            SoundManager.instance.SesEfektiCikar(1);
        }
    }

    public void EldekiSekliDegistir()
    {
        if (eldekiDegistirilebilirmi)
        {
            eldekiDegistirilebilirmi = false;

            aktifSekil.gameObject.SetActive(false);
            eldekiSekil.gameObject.SetActive(true);

            eldekiSekil.transform.position = aktifSekil.transform.position;

            aktifSekil = eldekiSekil;
        }

        if (takipShapeManager)
        {
            takipShapeManager.Reset();
        }
    }

    IEnumerator SeviyeGec()
    {
        yield return new WaitForSeconds(0.2f);

        int sayac = 0;

        while (sayac < seviyeAtlamaEfektleri.Length)
        {
            seviyeAtlamaEfektleri[sayac].EfectPlay();
            yield return new WaitForSeconds(0.1f);
            sayac++;
        }
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(0.2f);

        int sayac = 0;

        while (sayac < gameOverEfektleri.Length)
        {
            gameOverEfektleri[sayac].EfectPlay();
            yield return new WaitForSeconds(0.1f);
            sayac++;
        }

        yield return new WaitForSeconds(1f);

        if (gameOverPanel)
        {
            gameOverPanel.transform.localScale = Vector3.zero;
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBack);
        }
    }

    void Surukle(Vector2 suruklemeHareket)
    {
        suruklemeYonu = YonuBelirle(suruklemeHareket);
    }

    void SurukleBitti(Vector2 suruklemeHareket)
    {
        suruklemeBitisYonu = YonuBelirle(suruklemeHareket);
    }

    void Tap(Vector2 suruklemeHareket)
    {
        dokundumu = true;
    }

    Direction YonuBelirle(Vector2 suruklemHareket)
    {
        Direction suruklemeYonu = Direction.none;

        if(Mathf.Abs(suruklemHareket.x) > Mathf.Abs(suruklemHareket.y))
        {
            suruklemeYonu = (suruklemHareket.x >= 0) ? Direction.sag : Direction.sol;
        }
        else
        {
            suruklemeYonu = (suruklemHareket.y >= 0) ? Direction.yukari : Direction.asagi;
        }

        return suruklemeYonu;
    }
}
