using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScript : MonoBehaviour
{
    [SerializeField] private AudioClip musicLoop;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent <AudioSource>();
        audioSource.loop = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying) //Checks to see if the intro part of the music has finished
        {
            audioSource.clip = musicLoop;
            audioSource.Play();
            audioSource.loop = true;
            Destroy(GetComponent<MusicScript>());
            //Destroy the script as the check is unnessesary, 
            //since the looping part of the desired song will loop indefinetly as AudioSource.loop is true
        }
    }
}
