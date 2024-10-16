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

    public void SetPitch()
    {

        if (tag == "highpitchbutton")
        {
            float pitch = sc.GetPitch();
            freq.text = pitch.ToString();
            GlobalSpeed.rightPitch = pitch;

            Debug.Log(GlobalSpeed.rightPitch);
        }
        else if (tag == "lowpitchbutton")
        {
            float pitch = sc.GetPitch();
            freq.text = sc.GetPitch().ToString();
            GlobalSpeed.leftPitch = pitch;

            Debug.Log(GlobalSpeed.leftPitch);
        }

    }

}