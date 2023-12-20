using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Tuner : MonoBehaviour
{
    private const int SAMPLE_SIZE = 1024;
    private float pitchValue;
    public Text PitchValue;
    public Text Note;
    public Text DifferenceFreq;
    public Text Result;
    private int Struna;
    private float differenceFreq;
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

    private Dictionary<int, float> StrunaFreq = new Dictionary<int, float>()
    {
        { 1, 329.63f},  //E
        { 2, 246.94f},  //B
        { 3, 196.00f},	//G
        { 4, 146.83f},  //D
        { 5, 110.00f},  //A
        { 6,  82.41f}   //E
    };

    public void ButtonGetStrune(int struna)
    {
        Struna=struna;
    }

    public float FindDifferenceFreq()
    {
        Debug.Log(Struna);
        if (Struna==0)
        {
            Result.text = "Выберите струну";
            return 0;
        }
        var struneFreq = StrunaFreq[Struna];
        differenceFreq = pitchValue-struneFreq;
                           
        if (differenceFreq>1)
            Result.text = "Натянуть";
        else if (differenceFreq<-1)
            Result.text = "Ослабить";
        else
            Result.text = "Настроена";

        return differenceFreq;
    }

    // Start is called before the first frame update
    void Start()
    {
        sampleRate = AudioSettings.outputSampleRate;

        source = GetComponent<AudioSource>();

        string microphoneName =  Microphone.devices[0];
        source.outputAudioMixerGroup = MicrophoneGroup;        
    }

    public void OnRecords()
    {   
        if (Microphone.IsRecording(microphoneName)) Microphone.End(microphoneName);
        source.clip = Microphone.Start(microphoneName, true, 20, sampleRate);
        source.Play();      
    }

    public void OffRecords()
    {   
        Microphone.End(microphoneName);
        
        PitchValue.text = "Запись остановлена";
        Note.text = "Запись остановлена";
        DifferenceFreq.text = "Запись остановлена";
        Result.text = "Запись остановлена";
    }

    // Update is called once per frame
    void Update()
    {       
        if (Microphone.IsRecording(microphoneName))
            AnalyzeSound();       
        //Debug.Log(Struna); 
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

        return Note.text;
    }

    public void AnalyzeSound()
    {
        spectrum = new float[SAMPLE_SIZE];
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
        if (freqN>0f)
            pitchValue = freqN * (sampleRate / 2) / SAMPLE_SIZE; // convert index to frequency
            Note.text = GetNote(pitchValue).ToString();
            DifferenceFreq.text = FindDifferenceFreq().ToString();
        PitchValue.text = pitchValue.ToString();
    }
}
