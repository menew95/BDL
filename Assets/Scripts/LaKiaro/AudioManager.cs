using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public List<AudioClip> audioList = new List<AudioClip>();
    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallAudioClip(int _ID)
    {
        audioSource.clip = audioList[_ID];
        audioSource.Play();
    }
}
