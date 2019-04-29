using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    AudioSource audioSource;
    bool started = false;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(started && !audioSource.isPlaying) {
            Destroy(this.gameObject);
        }
    }

    public void SetClip(AudioClip newClip) {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = newClip;
        audioSource.Play();
        started = true;
    }
}
