using TMPro;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class SetGloballeftPitch : MonoBehaviour
{
    public SoundControll sc;
    public TextMeshProUGUI freq;
    AudioSource audioSource;
    public string selectedDevice;
    public AudioMixerGroup mixerGroupMicrophone;

    public float[] samples = new float[512]; // samplar om 20Hz - 20kHz till samples mellan [0,1024]


    public void SetPitch()
    {

        if (tag == "highpitchbutton")
        {
            float pitch = sc.GetPitch();
            freq.text = Mathf.Round(pitch).ToString() + " Hz";
            GlobalSpeed.rightPitch = pitch;
        }
        else if (tag == "lowpitchbutton")
        {
            float pitch = sc.GetPitch();
            freq.text = Mathf.Round(pitch).ToString() + " Hz";
            GlobalSpeed.leftPitch = pitch;
        }

    }

}