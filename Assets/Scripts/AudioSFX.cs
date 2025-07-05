using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSFX : MonoBehaviour
{
    public AudioSource audioSourceSFX;

    public AudioClip audioClipBenar;
    public AudioClip audioClipSalah;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SoundSFXBenar()
    {
        audioSourceSFX.PlayOneShot(audioClipBenar);
    }

    public void SoundSFXSalah()
    {
        audioSourceSFX.PlayOneShot(audioClipSalah);
    }
}
