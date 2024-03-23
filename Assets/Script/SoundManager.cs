using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private AudioClip[] musicClip;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource[] sesEfektleri;
    [SerializeField] private AudioSource[] vocalClips;

    AudioClip rastgeleMusicClip;
    public bool musicCalsinmi = true;
    public bool efektCalsinmi = true;

    public IconAcKapaManager musicIcon;
    public IconAcKapaManager fxIcon;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rastgeleMusicClip = RastgeleClipSec(musicClip);
        BackgroundMusicCal(rastgeleMusicClip);
    }

    AudioClip RastgeleClipSec(AudioClip[] clips)
    {
        AudioClip rastgeleClip = clips[Random.Range(0, clips.Length)];
        return rastgeleClip;
    }

    public void BackgroundMusicCal(AudioClip musicClip)
    {
        if (!musicClip || !musicSource || !musicCalsinmi)
        {
            return;
        }

        musicSource.volume = PlayerPrefs.GetFloat("musicVolume");
        musicSource.clip = musicClip;
        musicSource.Play();
    }

    void MusicGuncelle()
    {
        if (musicSource.isPlaying != musicCalsinmi)
        {
            if (musicCalsinmi)
            {
                rastgeleMusicClip = RastgeleClipSec(musicClip);
                BackgroundMusicCal(rastgeleMusicClip);
            }
            else
            {
                musicSource.Stop();
            }
        }
    }

    public void MusicAcKapa()
    {
        musicCalsinmi = !musicCalsinmi;
        MusicGuncelle();
        musicIcon.IconAcKapat(musicCalsinmi);
    }

    public void SesEfektiCikar(int hangiSes)
    {
        if (efektCalsinmi && hangiSes < sesEfektleri.Length)
        {
            sesEfektleri[hangiSes].volume = PlayerPrefs.GetFloat("fxVolume");
            sesEfektleri[hangiSes].Stop();
            sesEfektleri[hangiSes].Play();
        }
    }

    public void FXAcKapa()
    {
        efektCalsinmi = !efektCalsinmi;
        fxIcon.IconAcKapat(efektCalsinmi);
    }

    public void VocalSesiCikar()
    {
        if (efektCalsinmi)
        {
            AudioSource source = vocalClips[Random.Range(0, vocalClips.Length)];

            source.Stop();
            source.Play();
        }
    }
}
