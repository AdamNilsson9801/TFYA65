using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundControll : MonoBehaviour
{
    public GameObject car;
    AudioSource audioSource;

    public static float[] samples = new float[4096]; // samplar om 20Hz - 20kHz till samples mellan [0,1024]
    public static float[] samplesTimeDomain = new float[64];
    public static float[] freqBand = new float[8];
    public float[] totalRange = new float[8192];//8192 max size

    public AudioClip audioClip;
    public bool useMicrophone;
    public string selectedDevice;
    public AudioMixerGroup mixerGroupMicrophone, mixerGroupMaster;

    [Range(0f, 100f)]
    public float loudnessThreshold;
    public int sampleWindow = 64;

    //Harmonic Product Spectrum (HPS)
    public int harmonics = 5;


    private float[] sampleBuffer = new float[4096];
    private int frameCounter = 0;
    private int frameAmount = 30;
    // Start is called before the first frame update
    void Start()
    {

        //Debug.Log(sampleBuffer[0]);

        //Get audioSource
        audioSource = GetComponent<AudioSource>();

        //Get microphone
        SetAudioClip();

        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        //Detect pitch
        if (GetLoudness() > loudnessThreshold )
        {
            if (frameCounter < frameAmount)
            {
                GetSpectrumAudioSource();

                for (int i = 0; i < samples.Length; i++)
                {
                    sampleBuffer[i] += samples[i];
                }

                frameCounter++;
                Array.Clear(samples, 0, samples.Length);
            }


            if (frameCounter == frameAmount)
            {
                for (int i = 0; i < sampleBuffer.Length; i++)
                {
                    sampleBuffer[i] /= (float)frameAmount;
                }

                float detectedpitch = DetectPitch(sampleBuffer);
                Debug.Log("DETECTED PITCH: " + Mathf.Round(detectedpitch) + "Hz");
                frameCounter = 0;
                Array.Clear(sampleBuffer, 0, sampleBuffer.Length);

                car.GetComponent<Controlls>().ChangeLane(detectedpitch);
            }

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

        for (int h = 1; h <= harmonics; h++)
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

        float freq = maxIndex * AudioSettings.outputSampleRate / (2f * samples.Length);

        return freq;
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
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


    float GetLoudness()
    {
        int startPos = Microphone.GetPosition(Microphone.devices[0]) - sampleWindow;

        if (startPos < 0) startPos = 0;


        audioSource.clip.GetData(samplesTimeDomain, startPos);

        float totalLoudness = 0;
        for (int i = 0; i < sampleWindow; i++)
        {
            totalLoudness += Mathf.Abs(samplesTimeDomain[i]);
        }

        return Mathf.Round((totalLoudness / sampleWindow) * 1000);
    }
}
