using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeginStopAudio : MonoBehaviour
{
    public AudioSource audioSource;
    private bool start = false;
    // Start is called before the first frame update
    public void Switcher() {
        if (start) {
            start = false;
            audioSource.Stop();
        }
        else {
            start = true;
            audioSource.Play();
        }
    }
}
