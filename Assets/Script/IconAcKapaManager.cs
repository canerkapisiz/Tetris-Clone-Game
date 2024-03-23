using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconAcKapaManager : MonoBehaviour
{
    public Sprite acikIcon, kapaliIcon;

    private Image iconImg;

    public bool varsayilanIconDurumu = true;

    void Start()
    {
        iconImg = GetComponent<Image>();
        iconImg.sprite = (varsayilanIconDurumu) ? acikIcon : kapaliIcon;
    }

    public void IconAcKapat(bool iconDurumu)
    {
        if (!iconImg || !acikIcon || !kapaliIcon)
        {
            return;
        }

        iconImg.sprite = (iconDurumu) ? acikIcon : kapaliIcon;
    }
}
