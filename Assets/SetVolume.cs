using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void setLevel (float sliderValue)
    {
        mixer.SetFloat("MusicSlider", Mathf.Log10(sliderValue) * 20);
    }
}
