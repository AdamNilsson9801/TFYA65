using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundControll : MonoBehaviour
{
    public GameObject car;
    AudioSource audioSource;

    public static float[] samples = new float[2048]; // samplar om 20Hz - 20kHz till samples mellan [0,1024]
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
    public int harmonics = 4;


    private float[] sampleBuffer = new float[2048];
    private int frameCounter = 0;
    private int frameAmount = 10;
    //private int frameReset = 60;
    //private bool isMoving = false;

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
        // vi måste nollställa framecounter om getloudness blir lägre än loudnessThreshold
        //Detect pitch
        if (GetLoudness() > loudnessThreshold)
        {
            if (frameCounter < frameAmount) //Do this 15 frames
            {
                GetSpectrumAudioSource();

                for (int i = 0; i < samples.Length; i++) //Store values in buffer
                {
                    sampleBuffer[i] += samples[i];
                }
                frameCounter++;
                Array.Clear(samples, 0, samples.Length); //Clear samples array
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
        else
        {
            frameCounter = 0;
            Array.Clear(sampleBuffer, 0, sampleBuffer.Length);
        }
    }

    private float DetectPitch(float[] spectrum)
    {
        if (useHPS)
        {
            return HPS(spectrum);
        }
        else
        {
            return 0f;
        }

    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        float cutoffFrequency = 5000f; // Set the cutoff frequency
        float cutoffFrequency2 = 70f; // Set the cutoff frequency

        for (int i = 0; i < samples.Length; i++)
        {
            float frequency = i * AudioSettings.outputSampleRate / (2f * samples.Length);
            if (frequency > cutoffFrequency || frequency < cutoffFrequency2)
            {
                samples[i] = 0f; // Zero out values above the cutoff frequency
            }
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

    float HPS(float[] spectrum)
    {
        int length = spectrum.Length;
        float[] hps = new float[length];

        for (int i = 0; i < length; i++)
        {
            hps[i] = spectrum[i];
        }

        for (int h = 2; h <= harmonics; h++)
        {
            for (int i = 0; i < (length / h); i++)
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
    public float GetPitch()
    {
        GetSpectrumAudioSource();
        return DetectPitch(samples); 
    }
}
