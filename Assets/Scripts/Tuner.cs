using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Tuner : MonoBehaviour
{
    private const int SAMPLE_SIZE = 1024;
    public float pitchValue;
    public Text PitchValue;
    public Text Note;
    public AudioSource source;
    private float[] spectrum;
    private int sampleRate;
    private string microphoneName;
    public AudioMixerGroup MicrophoneGroup;

    private Dictionary<string, float> noteBaseFreqs = new Dictionary<string, float>()
    {
        { "C", 16.35f },
        { "C#", 17.32f },
        { "D", 18.35f },
        { "Eb", 19.45f },
        { "E", 20.60f },
        { "F", 21.83f },
        { "F#", 23.12f },
        { "G", 24.50f },
        { "G#", 25.96f },
        { "A", 27.50f },
        { "Bb", 29.14f },
        { "B", 30.87f },
    };

    // Start is called before the first frame update
    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;

        source = GetComponent<AudioSource>();

        string microphoneName =  Microphone.devices[0];
        source.outputAudioMixerGroup = MicrophoneGroup;
        // source.clip = Microphone.Start(microphoneName, true, 20, sampleRate);
        // source.Play();
        
        spectrum = new float[SAMPLE_SIZE];        
    }

    public void OnRecords()
    {   
        Microphone.End(microphoneName);
        source.clip = Microphone.Start(microphoneName, true, 20, sampleRate);
        source.Play();      
    }

    public void OffRecords()
    {   
        Microphone.End(microphoneName);
    }

    // Update is called once per frame
    void Update()
    {       
        AnalyzeSound();        
    }

    public string GetNote(float freq)
    {
        float baseFreq;

        foreach (var note in noteBaseFreqs)
        {
            baseFreq = note.Value;

            for (int i = 0; i < 9; i++)
            {
                if ((freq >= baseFreq - 0.5) && (freq < baseFreq + 0.485) || (freq == baseFreq))
                {
                    return note.Key + i;
                }

                baseFreq *= 2;
            }
        }

        return Note.text; //"---";
    }

    public void AnalyzeSound()
    {
        //Find Pitch
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
        
        float maxV = 0;
        var maxN = 0;
        float Threshold = 0.01f;
        for (var i = 0; i < SAMPLE_SIZE; i++)
        { // find max 
            if (!(spectrum[i] > maxV) || !(spectrum[i] > Threshold))         
                continue;
            maxV = spectrum[i];
            maxN = i; // maxN is the index of max
            
        }
        float freqN = maxN; // pass the index to a float variable
        if (maxN > 0 && maxN < SAMPLE_SIZE - 1)
        { // interpolate index using neighbours
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        if (freqN>0)
            pitchValue = freqN * (sampleRate / 2) / SAMPLE_SIZE; // convert index to frequency
            Note.text = GetNote(pitchValue).ToString();
        PitchValue.text = pitchValue.ToString();
    }
}
