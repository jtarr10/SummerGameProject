using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public struct Note
{
    public string value;
    public string type;
    public float duration;
    public float freq;
    public int bpm;
    Dictionary<string, float> freqPairs;
    Dictionary<string, float> typeDurations;

    /// <summary>
    /// This will initialize a note object with the given note value and type. The available note values are as follows:
    /// A1, A1#, B1, C1, C1#, D1, D1#, E1, F1, F1#, G1, G1#, A2, A2#, B2, C2, C2#, D2, D2#, E2, F2, F2#, G2, G2#, A3, A3#, B3, C3. If no note should be played,
    /// the user can pass the word "rest" as the noteValue. 
    /// The possible note types are as follows: 1/32, 1/16, 1/8, 1/4, 1/2, dotted-half, whole, 2-whole. The 1/4 note gets the beat as by convention of 4/4 time.
    /// </summary>
    /// <param name="noteValue">the note value string of this note object</param>
    /// <param name="noteType">Pass a string descriptor of the note type.</param>
    /// <param name="rate">Pass an int that indicates the bpm of the note to be played</param>
    public Note(string noteValue, string noteType, int rate)
    {
        value = noteValue;
        type = noteType;
        bpm = rate;

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

        //calculate the number of seconds per beat
        float beatLength = 60.0f / (float)bpm; 

        //setting up the dictionary of note durations depending on bpm
        typeDurations = new Dictionary<string, float>()
        {
            ["1/32"] = beatLength / 8.0f,
            ["1/16"] = beatLength / 4.0f,
            ["1/8"] = beatLength / 2.0f,
            ["1/4"] = beatLength,
            ["1/2"] = beatLength * 2.0f,
            ["dotted-half"] = beatLength * 3.0f,
            ["whole"] = beatLength * 4.0f,
            ["2-whole"] = beatLength * 8.0f
        };

        if(typeDurations.ContainsKey(type))
        {
            duration = typeDurations[type];
        }
        else
        {
            duration = beatLength;
        }

    }
}





public class MusicEngine : MonoBehaviour
{
    public int bpm;
    public Note key;
    Queue<Note> sequence;
    public Instrument lead;
    bool playingSequence;

    //Singleton Setup
    public static MusicEngine Instance { get; private set; }
    private void Awake()
    {
        //First time singleton setup 
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        playingSequence = false;
        lead = GameObject.Find("Lead Instrument").GetComponent<Instrument>();

        //creating a test sequence: Yankee Doodle     DELETE ME
        Note[] temp = new Note[]
        {
            new Note("C2", "1/4", 120),
            new Note("C2", "1/4", 120),
            new Note("D2", "1/4", 120),
            new Note("E2", "1/4", 120),
            new Note("C2", "1/4", 120),
            new Note("E2", "1/4", 120),
            new Note("D2", "1/4", 120),
            new Note("G1", "1/4", 120),
            new Note("C2", "1/4", 120),
            new Note("C2", "1/4", 120),
            new Note("D2", "1/4", 120),
            new Note("E2", "1/4", 120),
            new Note("C2", "1/2", 120),
            new Note("B2", "1/2", 120),
            new Note("C2", "1/4", 120),
            new Note("C2", "1/4", 120),
            new Note("D2", "1/4", 120),
            new Note("E2", "1/4", 120),
            new Note("F2", "1/4", 120),
            new Note("E2", "1/4", 120),
            new Note("D2", "1/4", 120),
            new Note("C2", "1/4", 120),
            new Note("B2", "1/4", 120),
            new Note("G1", "1/4", 120),
            new Note("A2", "1/4", 120),
            new Note("B2", "1/4", 120),
            new Note("C2", "1/2", 120),
            new Note("C2", "1/2", 120)
        };

        sequence = new Queue<Note>(temp);

        PlaySequence(sequence);  //DELETE ME
    }


    public void PlaySequence(Queue<Note> notes)
    {
        sequence = notes;

        if(lead.isPlaying != true)
        {
            playingSequence = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

        //playing the sequence queue
        if(playingSequence && (lead.isPlaying != true))
        {
            if(sequence.Count == 0)
            {
                playingSequence = false;
            }
            else
            {
                lead.Play(sequence.Dequeue());
            }
        }


    }
}
