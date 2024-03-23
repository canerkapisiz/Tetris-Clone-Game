using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem[] tumEfekfler;

    void Start()
    {
        tumEfekfler = GetComponentsInChildren<ParticleSystem>();
    }

    public void EfectPlay()
    {
        foreach (ParticleSystem efect in tumEfekfler)
        {
            efect.Stop();
            efect.Play();
        }
    }
}
