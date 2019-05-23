using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;




public class Instrument : MonoBehaviour
{
    string[] noteValues = new string[] { "A1", "A1#", "B1", "C1", "C1#", "D1", "D1#", "E1", "F1", "F1#", "G1", "G1#", "A2", "A2#", "B2", "C2", "C2#", "D2", "D2#", "E2", "F2", "F2#", "G2", "G2#", "A3", "A3#", "B3", "C3" };
    public string soundName = "Piano";
    string soundPath;
    float volume = 1.0f;
    public float pitchModifier = 1.0f;
    bool isPlaying = false;
    AudioSource output;


    public int counter = 0; //delete me

    // Start is called before the first frame update
    void Start()
    {
        output = gameObject.GetComponent<AudioSource>();
        soundPath = "Audio\\" + soundName;
    }

    /// <summary>
    /// This function will play the instrument in a loop on the given note
    /// </summary>
    void Play(Note key)
    {
        if (key.value != "rest")
        {
            isPlaying = true;
            pitchModifier = key.freq;
            output.PlayOneShot(Resources.Load<AudioClip>(soundName));
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
        //Test Code: PLEASE DELETE WHEN DONE
        if(Input.GetKeyUp(KeyCode.Space) && isPlaying == false)
        {
            Note p = new Note(noteValues[counter], 1.0f);
            Play(p);
        }
        else if(Input.GetKeyUp(KeyCode.Space) && isPlaying == true)
        {
            Stop();
        }

        if(Input.GetKeyUp(KeyCode.W))
        {
            counter++;
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            counter--;
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            counter = 0;
            Timer time = gameObject.AddComponent<Timer>();
            time.StartTimer(5f);
        }

    }
}
