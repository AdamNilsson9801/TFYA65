using TMPro;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SetGloballeftPitch : MonoBehaviour
{
    public TextMeshProUGUI freq;
    AudioSource audioSource;
    public string selectedDevice;
    public AudioMixerGroup mixerGroupMicrophone;

    public float[] samples = new float[512]; // samplar om 20Hz - 20kHz till samples mellan [0,1024]

    private int harmonics = 5;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();       
        SetAudioClip();
        audioSource.Play();
    }

    private void Update()
    {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);
    }
    public void SetLeftPitch()
    {

        if (tag == "highpitchbutton")
        {
            freq.text = "180 Hz";
        }
        else if(tag == "lowpitchbutton")
        {
            freq.text = "120 Hz";
        }

        ////Get microphone
        //float pitch = GetPitch(samples);
        //GlobalSpeed.leftPitch = Mathf.Round(pitch);
        //leftFreq.text = Mathf.Round(pitch).ToString() + " Hz";

        //audioSource.Stop();

    }

    float GetPitch(float[] samples)
    {
        float freq = HPS(samples);
        Debug.Log(freq);
        return freq;
    }

    float HPS(float[] spectrum)
    {
        int length = spectrum.Length;
        float[] hps = new float[length];

        //Get max
        float maxValue = float.MinValue;
        for (int i = 0; i < length; i++)
        {
            if (spectrum[i] > maxValue)
            {
                maxValue = spectrum[i];
            }
        }

        for (int i = 0; i < length; i++)
        {
            hps[i] = spectrum[i] / maxValue;
        }

        for (int h = 2; h <= harmonics; h++)
        {
            for (int i = 0; i < length / h; i++)
            {
                hps[i] += spectrum[i * h];
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

    void SetAudioClip()
    {

        if (Microphone.devices.Length > 0)
        {
            selectedDevice = Microphone.devices[0].ToString();
            audioSource.outputAudioMixerGroup = mixerGroupMicrophone;
            audioSource.clip = Microphone.Start(selectedDevice, true, 100, AudioSettings.outputSampleRate);
        }
        else
        {
            Debug.Log("No microphone was found");
        }
    }
}