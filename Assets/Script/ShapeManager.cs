using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeManager : MonoBehaviour
{
    [SerializeField] private bool donebilirMi = true;

    public Sprite shapeSekil;

    GameObject[] yerlesmeEfektleri;

    void Start()
    {
        yerlesmeEfektleri = GameObject.FindGameObjectsWithTag("yerlesmeEfekti");
    }

    public void YerlesmeEfektleriCikar()
    {
        int sayac = 0;

        foreach (Transform child in gameObject.transform)
        {
            if (yerlesmeEfektleri[sayac])
            {
                yerlesmeEfektleri[sayac].transform.position = new Vector3(child.position.x, child.position.y, 0f);

                ParticleManager particleManager = yerlesmeEfektleri[sayac].GetComponent<ParticleManager>();

                if (particleManager)
                {
                    particleManager.EfectPlay();
                }
            }
            sayac++;
        }
    }

    public void SolaHareket()
    {
        transform.Translate(Vector3.left, Space.World);
    }

    public void SagaHareket()
    {
        transform.Translate(Vector3.right, Space.World);
    }

    public void AsagiHareket()
    {
        transform.Translate(Vector3.down, Space.World);
    }

    public void YukariHareket()
    {
        transform.Translate(Vector3.up, Space.World);
    }

    public void SagaDon()
    {
        if (donebilirMi)
        {
            transform.Rotate(0, 0, -90);
        }
    }

    public void SolaDon()
    {
        if (donebilirMi)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    public void SaatYonundeDonsun(bool saatYonumu)
    {
        if (saatYonumu)
        {
            SagaDon();
        }
        else
        {
            SolaDon();
        }
    }
}
