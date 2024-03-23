using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField]
    private Transform tilePrefab;

    public int yukseklik = 22;
    public int genislik = 10;

    private Transform[,] izgara;

    public int tamamlananSatir = 0;

    public ParticleManager[] satirEfekti = new ParticleManager[4];

    private void Awake()
    {
        izgara = new Transform[genislik, yukseklik];
    }

    void Start()
    {
        BosKareleriOlustur();
    }

    bool BoardIcindemi(int x, int y)
    {
        return (x >= 0 && x < genislik && y >= 0);
    }

    bool KareDolumu(int x, int y, ShapeManager shape)
    {
        return (izgara[x, y] != null && izgara[x, y].parent != shape.transform);
    }

    public bool GecerliPozisyondami(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectoruIntYap(child.position);

            if (!BoardIcindemi((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (pos.y < yukseklik)
            {
                if (KareDolumu((int)pos.x, (int)pos.y, shape))
                {
                    return false;
                }
            }

        }

        return true;
    }

    void BosKareleriOlustur()
    {
        for (int y = 0; y < yukseklik; y++)
        {
            for (int x = 0; x < genislik; x++)
            {
                Transform tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.name = "x " + x.ToString() + " ," + " y" + y.ToString();
                tile.parent = this.transform;
            }
        }
    }

    Vector2 VectoruIntYap(Vector2 vector)
    {
        return new Vector2(Mathf.Round(vector.x), Mathf.Round(vector.y));
    }

    public void SekliIzgaraIcineAl(ShapeManager shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = VectoruIntYap(child.position);
            izgara[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool SatirTamamlandimi(int y)
    {
        for (int x = 0; x < genislik; ++x)
        {
            if (izgara[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void SatiriTemizle(int y)
    {
        for (int x = 0; x < genislik; ++x)
        {
            if (izgara[x, y].gameObject)
            {
                Destroy(izgara[x, y].gameObject);
            }

            izgara[x, y] = null;
        }
    }

    void BirSatirAsagiIndir(int y)
    {
        for (int x = 0; x < genislik; ++x)
        {
            if (izgara[x, y] != null)
            {
                izgara[x, y - 1] = izgara[x, y];
                izgara[x, y] = null;
                izgara[x, y - 1].position += Vector3.down;
            }
        }
    }

    void TumSatirlariAsagiIndir(int baslangicY)
    {
        for (int i = baslangicY; i < yukseklik; ++i)
        {
            BirSatirAsagiIndir(i);
        }
    }

    public IEnumerator TumSatirlariTemizle()
    {
        tamamlananSatir = 0;
        for (int y = 0; y < yukseklik; ++y)
        {
            if (SatirTamamlandimi(y))
            {
                SatirEfektiniCalistir(tamamlananSatir, y);
                tamamlananSatir++;
            }
        }
        yield return new WaitForSeconds(0.5f);
        for (int y = 0; y < yukseklik; y++)
        {
            if (SatirTamamlandimi(y))
            {
                SatiriTemizle(y);
                TumSatirlariAsagiIndir(y + 1);
                yield return new WaitForSeconds(0.2f);
                y--;
            }
        }
    }

    public bool DisariTastimi(ShapeManager shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= yukseklik - 1)
            {
                return true;
            }
        }

        return false;
    }

    void SatirEfektiniCalistir(int kacinciSayi,int y)
    {
        if (satirEfekti[kacinciSayi])
        {
            satirEfekti[kacinciSayi].transform.position = new Vector3(0, y, 0);
            satirEfekti[kacinciSayi].EfectPlay();
        }
    }
}
