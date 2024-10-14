using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SoundControll : MonoBehaviour
{
    public GameObject car;
    AudioSource audioSource;
    public TextMeshProUGUI text;

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
    public int harmonics = 5;


    private float[] sampleBuffer = new float[4096];
    private int frameCounter = 0;
    private int frameAmount = 20;
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
                //Mean spectrum over 15 frames
                for (int i = 0; i < sampleBuffer.Length; i++)
                {
                    sampleBuffer[i] /= (float)frameAmount;
<<<<<<< Updated upstream
=======
                    //Debug.Log(sampleBuffer[i] + " sampleBuffer");
>>>>>>> Stashed changes
                }

                float detectedpitch = DetectPitch(sampleBuffer);

                if (text)
                {
                    text.text = Mathf.Round(detectedpitch).ToString() + "Hz";
                }
                frameCounter = 0;
                Array.Clear(sampleBuffer, 0, sampleBuffer.Length);

                if (car)
                {
                    car.GetComponent<Controlls>().ChangeLane(detectedpitch);
                }
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
            return AutoCorrelation(spectrum);
        }

    }

    private float AutoCorrelation(float[] signal)
    {
        int signalLength = signal.Length;
        float[] autocorr = new float[signalLength];

        // Compute autocorrelation for each lag
        for (int lag = 0; lag < signalLength; lag++)
        {
            float sum = 0f;
            for (int i = 0; i < signalLength - lag; i++)
            {
                sum += signal[i] * signal[i + lag];
            }
            autocorr[lag] = sum;
        }

        // Ignore lag 0, find first significant peak after it
        int maxIndex = 1; // Start from 1 to avoid the lag 0 peak
        float maxValue = autocorr[1];

        for (int i = 2; i < signalLength; i++)
        {
            if (autocorr[i] > maxValue)
            {
                maxValue = autocorr[i];
                maxIndex = i;
            }
        }

        // Convert the index of the peak to frequency
        float sampleRate = AudioSettings.outputSampleRate;
        float freq = sampleRate / maxIndex; // maxIndex corresponds to the period

        return freq;
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

<<<<<<< Updated upstream
        float cutoffFrequency = 1000f; // Set the cutoff frequency
        float cutoffFrequency2 = 70f; // Set the cutoff frequency
=======
        float cutoffFrequency = 16000f; // Set the cutoff frequency
        float cutoffFrequency2 = 60f; // Set the cutoff frequency
>>>>>>> Stashed changes

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

        for (int h = 1; h <= harmonics; h++)
        {
            for (int i = 0; i < length / h; i++)
            {
                hps[i] += spectrum[i * h];
            }
        }

<<<<<<< Updated upstream
        float maxVal = 0f;
        int maxIndex = 0;

        for (int i = 0; i < length; i++)
=======
        //float maxVal = hps.Max();
        //int maxIndex = Array.IndexOf(hps, maxVal);

        //float specMaxVal = spectrum.Max();
        //Debug.Log("Detected max-value hps: " + maxVal);
        //Debug.Log("Detected max-value in spectrum: " + specMaxVal);
        //Debug.Log("Spectrum test");
        //Debug.Log("Detected hps maxIndex in spectrum: " + spectrum[maxIndex]);
        //Debug.Log("Detected hps maxIndex*2 in spectrum: " + spectrum[maxIndex*2]);
        //Debug.Log("Detected hps maxIndex*3 in spectrum: " + spectrum[maxIndex*3]);
        //float spectrumMax = (spectrum[maxIndex] * spectrum[maxIndex * 2] * spectrum[maxIndex * 3]);
        //Debug.Log("Calculated max-value in spectrum from hps maxIndex(1,2,3): " + spectrumMax);

        float maxVal = 0f;
        int maxIndex = 0;

        for (int i = 0; i < length/harmonics; i++)
>>>>>>> Stashed changes
        {
            if (hps[i] > maxVal)
            {
                maxVal = hps[i];
                maxIndex = i;
            }
        }

        float freq = maxIndex * AudioSettings.outputSampleRate / (2f * samples.Length);

<<<<<<< Updated upstream
=======
        Debug.Log("Detected max-value hps: " + maxVal);
        Debug.Log("Detected maxIndex: " + maxIndex);
        Debug.Log("Detected frequency: " + freq + " Hz");

>>>>>>> Stashed changes
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
