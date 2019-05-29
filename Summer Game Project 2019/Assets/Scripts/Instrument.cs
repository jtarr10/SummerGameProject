using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;




public class Instrument : MonoBehaviour
{
    string[] noteValues = new string[] { "A1", "A1#", "B1", "C1", "C1#", "D1", "D1#", "E1", "F1", "F1#", "G1", "G1#", "A2", "A2#", "B2", "C2", "C2#", "D2", "D2#", "E2", "F2", "F2#", "G2", "G2#", "A3", "A3#", "B3", "C3" };
    public string soundName = "Piano";
    string soundPath;
    public float pitchModifier = 1.0f;
    public bool isPlaying = false;
    AudioSource output;
    protected Timer timer;




    // Start is called before the first frame update
    void Start()
    {
        output = gameObject.GetComponent<AudioSource>();
        soundPath = "Audio\\" + soundName;
        timer = gameObject.AddComponent<Timer>();
    }

    /// <summary>
    /// This function will play the given note on the instrument
    /// </summary>
    public void Play(Note key)
    {
        if (key.value != "rest")
        {
            isPlaying = true;
            pitchModifier = key.freq;
            output.PlayOneShot(Resources.Load<AudioClip>(soundPath));
            timer.StartTimer(key.duration);
        }
        else
        {
            isPlaying = true;
            timer.StartTimer(key.duration);
        }
    }


    /// <summary>
    /// This function will stop playback of the sound loop
    /// </summary>
    void Stop()
    {
        isPlaying = false;
        output.Stop();
    }


    /// <summary>
    /// This will set the volume of the instrument to level
    /// </summary>
    /// <param name="level">the level the volume will be set to (from 0 to 1)</param>
    void setVolume(float level)
    {
        if(level <= 1.0f && level >= 0.0f)
        {
            output.volume = level;
        }
        else
        {
            Debug.Log("setVolume should be set between 0 and 1");
        }
        
    }


    // Update is called once per frame
    void Update()
    {
        output.pitch = pitchModifier;


        //finishing note when the timer is done
        if(timer.FinishCount())
        {
            Stop();
        }
    }
}
