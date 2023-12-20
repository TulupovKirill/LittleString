using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Runtime.CompilerServices;

public class Metronome : MonoBehaviour
{
    public Slider ultraSlider;
    public Text text;
    public double bpm = 125.0F;

    double nextTick = 0.0F; // The next tick in dspTime
    double sampleRate = 0.0F;
    bool ticked = false;
    private AudioSource audioSource;

    void Start()
    {
        text.text = bpm.ToString();
        audioSource = GetComponent<AudioSource>();
        double startTick = AudioSettings.dspTime;
        sampleRate = AudioSettings.outputSampleRate;

        nextTick = startTick + (60.0 / bpm);
    }

    void LateUpdate()
    {
        if (!ticked && nextTick >= AudioSettings.dspTime)
        {
            ticked = true;
            BroadcastMessage("OnTick");
        }
    }

    // Just an example OnTick here
    void OnTick()
    {
        //Debug.Log("Tick");
        GetComponent<AudioSource>().Play();
    }

    void FixedUpdate()
    {
        double timePerTick = 60.0f / bpm;
        double dspTime = AudioSettings.dspTime;

        while (dspTime >= nextTick)
        {
            ticked = false;
            nextTick += timePerTick;
        }

    }

    public void ButClickUp()
    {
        text.text = bpm.ToString();
        bpm++;
        Debug.Log(bpm);
    }

    public void ButClickDn()
    {
        text.text = bpm.ToString();
        bpm--;
        Debug.Log(bpm);
    }

    public void SliderMan()
    {
        float aga = ultraSlider.value;
        Debug.Log(aga);
    }
}
