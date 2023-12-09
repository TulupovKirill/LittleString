using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public Dictionary<string, AudioSource> soundCollection = new Dictionary<string, AudioSource>();
    public GameObject AudioSet;
    // Start is called before the first frame update
    void Start()
    {
        AudioSet = GameObject.Find("Audioset");
        
        GameObject originalGameObject = Instantiate(AudioSet);
        for (int i = 0; i < originalGameObject.transform.childCount; i++)
        {
            GameObject child = originalGameObject.transform.GetChild(i).gameObject;
            soundCollection.Add(child.name, child.GetComponent<AudioSource>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playThisSoundEffect(string note){
        soundCollection[note].Play();
    }
}
