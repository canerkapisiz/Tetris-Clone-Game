using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    int skor = 0;
    int satirlar;
    public int level = 1;

    public int seviyedekiSatirSayisi = 5;

    int minSatir = 1;
    int maxSatir = 4;

    public TextMeshProUGUI satirText;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI skorText;

    public bool levelGecildimi = false;

    void Start()
    {
        Reset();
    }

    public void Reset()
    {
        level = 1;
        satirlar = seviyedekiSatirSayisi * level;
        TextGuncelle();
    }

    public void SatirSkoru(int n)
    {
        levelGecildimi = false;
        n = Mathf.Clamp(n, minSatir, maxSatir);

        switch (n)
        {
            case 1:
                skor += 30 * level;
                break;
            case 2:
                skor += 50 * level;
                break;
            case 3:
                skor += 150 * level;
                break;
            case 4:
                skor += 500 * level;
                break;
        }
        satirlar -= n;
        if (satirlar <= 0)
        {
            LevelAtla();
        }
        TextGuncelle();
    }

    void TextGuncelle()
    {
        if (skorText)
        {
            skorText.text = BasaSifirEkle(skor, 5);
        }

        if (levelText)
        {
            levelText.text = level.ToString();
        }

        if (satirText)
        {
            satirText.text = satirlar.ToString();
        }
    }

    string BasaSifirEkle(int skor, int rakamSayisi)
    {
        string skorStr = skor.ToString();

        while (skorStr.Length < rakamSayisi)
        {
            skorStr = "0" + skorStr;
        }

        return skorStr;
    }

    public void LevelAtla()
    {
        level++;
        satirlar = seviyedekiSatirSayisi * level;
        levelGecildimi = true;
    }
}
