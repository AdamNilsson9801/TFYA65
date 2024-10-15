using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class SoundControll : MonoBehaviour
{
    public GameObject car;
    AudioSource audioSource;
    public TextMeshProUGUI text;

    public int sampleLength = 4096;
    public static float[] samples = new float[4096]; // samplar om 20Hz - 20kHz till samples mellan [0,1024]
    public static float[] samplesTimeDomain = new float[64];
    //public static float[] freqBand = new float[8];
    public float[] totalRange = new float[8192];//8192 max size

    public AudioClip audioClip;
    public bool useMicrophone;
    public string selectedDevice;
    public AudioMixerGroup mixerGroupMicrophone, mixerGroupMaster;

    [Range(0f, 100f)]
    public float loudnessThreshold;
    public int sampleWindow = 64;
    public bool useHPS = true;

    //Harmonic Product Spectrum (HPS)
    public int harmonics = 3;


    // Start is called before the first frame update
    void Start()
    {
   
        //Get microphone
        SetAudioClip();

        audioSource.Play();
    }

    float DetectPitch()
    {
        if (useHPS)
        {
            return HPS();
        }
        else
        {
            return 0f;
        }

    }



    private float[] GetSpectrumAudioSource()
    {
        float[] spectrum = new float[sampleLength];

        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Blackman);

        float cutoffFrequency = 10000f; // Set the cutoff frequency
        float cutoffFrequency2 = 70f; // Set the cutoff frequency

        for (int i = 0; i < spectrum.Length; i++)
        {
            float frequency = i * AudioSettings.outputSampleRate / (2f * spectrum.Length);
            if (frequency > cutoffFrequency || frequency < cutoffFrequency2)
            {
                spectrum[i] = 0f; // Zero out values above the cutoff frequency
            }
        }

        return spectrum;
    }


    void SetAudioClip()
    {
        if (useMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                //Get audioSource
                audioSource = GetComponent<AudioSource>();

                selectedDevice = Microphone.devices[0];
                audioSource.outputAudioMixerGroup = mixerGroupMicrophone;
                audioSource.clip = Microphone.Start(selectedDevice, true, 100, AudioSettings.outputSampleRate);

                while (!(Microphone.GetPosition(selectedDevice) > 0)) { }

                audioSource.loop = true;
            }
            else
            {
                Debug.Log("Error! Microphone not found.");
                useMicrophone = false;
            }
        }

        if (!useMicrophone)
        {
            audioSource.clip = audioClip;
            audioSource.outputAudioMixerGroup = mixerGroupMaster;
            audioSource.loop = true;
        }
    }

    float HPS()
    {

        float[] spectrum = GetSpectrumAudioSource();

        int spectrumLength = spectrum.Length;
        float[] hps = new float[spectrumLength];

        for (int i = 0; i < spectrumLength; i++)
        {
            hps[i] = spectrum[i];
        }

        for (int h = 2; h <= harmonics; h++)
        {
            for (int i = 0; i < spectrumLength / h; i++)
            {
                hps[i] *= spectrum[i * h];
            }
        }

        float maxVal = 0f;
        int maxIndex = 0;

        for (int i = 0; i < spectrumLength / harmonics; i++)
        {
            if (hps[i] > maxVal)
            {
                maxVal = hps[i];
                maxIndex = i;
            }
        }

        float freq = maxIndex * AudioSettings.outputSampleRate / (2f * samples.Length);
        //Debug.Log("Freq: " + freq);
        return freq;
    }


    public float GetLoudness()
    {
        int startPos = Microphone.GetPosition(Microphone.devices[0]) - sampleWindow;

        if (startPos < 0) return 0f;

        float[] samplesTimeDomain = new float[sampleWindow];
        audioSource.clip.GetData(samplesTimeDomain, startPos);

        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(samplesTimeDomain[i]);
        }

        return (totalLoudness / sampleWindow) * 1000f;
    }

    public float GetPitch()
    {
        if (GetLoudness() > loudnessThreshold)
        {
            return DetectPitch();
        }

        return 0f;
    }
}
