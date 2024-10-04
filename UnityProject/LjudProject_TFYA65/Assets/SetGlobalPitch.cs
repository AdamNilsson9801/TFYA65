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

    private int harmonics = 5;

    private void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        //audioSource.Play();
    }


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