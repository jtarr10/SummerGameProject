using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Instrument : MonoBehaviour
{

    string soundName = "Audio\\" + "Piano";
    float volume = 1.0f;
    public float pitchModifier = 1.0f;
    bool isPlaying = false;
    AudioSource output;

    // Start is called before the first frame update
    void Start()
    {
        output = gameObject.GetComponent<AudioSource>();

    }

    void Play()
    {
        isPlaying = true;
        output.PlayOneShot(Resources.Load<AudioClip>(soundName));
    }

    void Stop()
    {
        isPlaying = false;
        output.Stop();
    }


    // Update is called once per frame
    void Update()
    {
        output.pitch = pitchModifier;
        //Test Code: PLEASE DELETE WHEN DONE
        if(Input.GetKeyUp(KeyCode.Space) && isPlaying == false)
        {
            Play();
        }
        else if(Input.GetKeyUp(KeyCode.Space) && isPlaying == true)
        {
            Stop();
        }

        if(Input.GetKeyUp(KeyCode.W))
        {
            pitchModifier += 0.01f;
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            pitchModifier -= 0.01f;
        }
        if(Input.GetKeyUp(KeyCode.X))
        {
            pitchModifier = 1.0f;
        }

    }
}
