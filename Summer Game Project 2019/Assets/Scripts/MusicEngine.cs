using System.Collections;
using System.Collections.Generic;
using UnityEngine;



struct Note
{
    public string value;
    public float duration;
    public float freq;
    Dictionary<string, float> freqPairs;

    /// <summary>
    /// This will initialize a note object with the given note value and duration (in seconds). The available note values are as follows:
    /// A1, A1#, B1, C1, C1#, D1, D1#, E1, F1, F1#, G1, G1#, A2, A2#, B2, C2, C2#, D2, D2#, E2, F2, F2#, G2, G2#, A3, A3#, B3, C3. If no note should be played,
    /// the user can pass the word "rest" as the noteValue.
    /// </summary>
    /// <param name="noteValue">the note value string of this note object</param>
    /// <param name="length">note duration in seconds</param>
    public Note(string noteValue, float length)
    {
        value = noteValue;
        duration = length;

        //setting up the frequency pairs with measured values
        freqPairs = new Dictionary<string, float>()
        {
            ["C3"] = 2.00f,
            ["B3"] = 1.905f,
            ["A3#"] = 1.785f,
            ["A3"] = 1.685f,
            ["G2#"] = 1.59f,
            ["G2"] = 1.5f,
            ["F2#"] = 1.415f,
            ["F2"] = 1.33f,
            ["E2"] = 1.26f,
            ["D2#"] = 1.19f,
            ["D2"] = 1.12f,
            ["C2#"] = 1.06f,
            ["C2"] = 1.00f,
            ["B2"] = 0.943f,
            ["A2#"] = 0.89f,
            ["A2"] = 0.84f,
            ["G1#"] = 0.795f,
            ["G1"] = 0.75f,
            ["F1#"] = 0.713f,
            ["F1"] = 0.671f,
            ["E1"] = 0.63f,
            ["D1#"] = 0.598f,
            ["D1"] = 0.56f,
            ["C1#"] = 0.53f,
            ["C1"] = 0.5f,
            ["B1"] = 0.47f,
            ["A1#"] = 0.448f,
            ["A1"] = 0.42f
        };

        if(value != "rest")
        {
            freq = freqPairs[value];
        }
        else
        {
            freq = 0f;
        }
        

    }
}





public class MusicEngine : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
