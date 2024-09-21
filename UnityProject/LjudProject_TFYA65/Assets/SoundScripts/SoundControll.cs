using System;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundControll : MonoBehaviour
{
    AudioSource audioSource;

    public static float[] samples = new float[1024]; // samplar om 20Hz - 20kHz till samples mellan [0,1024]
    public static float[] freqBand = new float[8];
    public float[] totalRange = new float[8192];//8192 max size

    public AudioClip audioClip;
    public bool useMicrophone;
    public string selectedDevice;
    public AudioMixerGroup mixerGroupMicrophone, mixerGroupMaster;

    //Harmonic Product Spectrum (HPS)
    public int harmonics = 5;
    private float timeCounter = 0f; // Time counter for detecting every 2 seconds
    private float detectionInterval = 0.5f; // 2 seconds interval

    // Start is called before the first frame update
    void Start()
    {
        //Get audioSource
        audioSource = GetComponent<AudioSource>();

        //Get microphone
        SetAudioClip();

        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {


        //Extract the first 1024 samples
        //Array.Copy(totalRange, 0, samples, 0, samples.Length);

        //Detect pitch

        // Accumulate time since last update
        timeCounter += Time.deltaTime;

        // Check if 2 seconds have passed
        if (timeCounter >= detectionInterval)
        {
            // Reset the timer
            timeCounter = 0f;
            GetSpectrumAudioSource();
            MakeFrequencyBands();

            float detectedpitch = DetectPitch(samples);
            Debug.Log(detectedpitch);
        }
    }

    private float DetectPitch(float[] spectrum)
    {
        int length = spectrum.Length;
        float[] hps = new float[length];

        for (int i = 0; i < length; i++)
        {
            hps[i] = spectrum[i];
        }

        for (int h = 2; h <= harmonics; h++)
        {
            for (int i = 0; i < length / h; i++)
            {
                hps[i] *= spectrum[i * h];
            }
        }

        float maxVal = 0f;
        int maxIndex = 0;

        for (int i = 0; i < length; i++)
        {
            if (hps[i] > maxVal)
            {
                maxVal = hps[i];
                maxIndex = i;
            }
        }

        float freq = maxIndex * AudioSettings.outputSampleRate / (2 * samples.Length);

        return freq;
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
    void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < freqBand.Length; i++)
        {
            float avg = 0;
            int sampleCount = (int)Mathf.Pow(2, i) * 2;

            if (i == 7)
            {
                sampleCount += 2;
            }

            for (int j = 0; j < sampleCount; j++)
            {
                avg += samples[count] * (count + 1);
                count++;
            }

            avg /= count;
            freqBand[i] = avg * 10;

        }
    }

    void SetAudioClip()
    {
        if (useMicrophone)
        {
            if (Microphone.devices.Length > 0)
            {
                selectedDevice = Microphone.devices[0].ToString();
                audioSource.outputAudioMixerGroup = mixerGroupMicrophone;
                audioSource.clip = Microphone.Start(selectedDevice, true, 100, AudioSettings.outputSampleRate);
            }
            else
            {
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


}
