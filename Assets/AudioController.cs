using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    //public AudioSource pause;
    //public AudioSource unPause;
    //public AudioSource selectButton;
    //public AudioSource gameStart;
    //public AudioSource gameEnd;
    //public AudioSource money;
    //public AudioSource extraTime;
    //public AudioSource doorBell;

    public AudioSource mainSong;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("music", 1) == 0)
        {
            mainSong.Pause();
        }

        if (PlayerPrefs.GetFloat("volume", 1) < 1)
        {
            mainSong.volume = PlayerPrefs.GetFloat("volume");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlaySound(string soundName)
    {
        if (soundName == "Victory")
        {
            mainSong.Pause();
        }

        GameObject soundGo = GameObject.Find(soundName);
        AudioSource sound = soundGo.GetComponent<AudioSource>();
        sound.Play();
    }

}
