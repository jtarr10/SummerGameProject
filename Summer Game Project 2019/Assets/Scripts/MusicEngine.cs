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


public struct Scale
{
    public string root;
    public List<Note> scale;
    string[] noteValues;
    Dictionary<string, string> stepPatterns;

    /// <summary>
    /// A scale generates a list of notes that form an octave of a specified scale type based from the root. 
    /// Scale.scale is the list of notes
    /// </summary>
    /// <param name="rootNote">This is the root note of the scale. The highest note value that will work is C2. </param>
    /// <param name="type">The scale can be either major or minor. Pass one of the two strings to the type parameter.</param>
    public Scale(string rootNote, string type)
    {
        root = rootNote;

        //setting up the note values of the two octaves of chromatic scale
        noteValues = new string[] { "C1", "C1#", "D1", "D1#", "E1", "F1", "F1#", "G1", "G1#", "A2", "A2#", "B2", "C2", "C2#", "D2", "D2#", "E2", "F2", "F2#", "G2", "G2#", "A3", "A3#", "B3", "C3" };

        //setting up step patterns
        stepPatterns = new Dictionary<string, string>
        {
            ["major"] = "WWHWWWH",
            ["minor"] = "WHWWHWW"
        };

        scale = new List<Note>();
        scale.Add(new Note(root, "1/4", 120));

        var notePointer = System.Array.IndexOf(noteValues, root);
        foreach(char letter in stepPatterns[type])
        {
            if(letter == 'W')
            {
                notePointer += 2;
            }
            else
            {
                notePointer += 1;
            }

            scale.Add(new Note(noteValues[notePointer], "1/4", 120));
        }
    }


}


public class MusicEngine : MonoBehaviour
{
    public int bpm;
    public Note root;
    Queue<Note> sequence;
    public Instrument lead;
    bool playingSequence;
    string[] possibleScales;

    int counter; //DELETE ME
    string[] types = new string[] { "major", "minor" };
    bool isMajor = true;

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

        possibleScales = new string[] { "C1", "C1#", "D1", "D1#", "E1", "F1", "F1#", "G1", "G1#", "A2", "A2#", "B2", "C2" };
        counter = 0; //DELETE ME
        

        ////test sequence: Yankee Doodle     DELETE ME
        //Note[] temp = new Note[]
        //{
        //    new Note("C2", "1/4", 120),
        //    new Note("C2", "1/4", 120),
        //    new Note("D2", "1/4", 120),
        //    new Note("E2", "1/4", 120),
        //    new Note("C2", "1/4", 120),
        //    new Note("E2", "1/4", 120),
        //    new Note("D2", "1/4", 120),
        //    new Note("G1", "1/4", 120),
        //    new Note("C2", "1/4", 120),
        //    new Note("C2", "1/4", 120),
        //    new Note("D2", "1/4", 120),
        //    new Note("E2", "1/4", 120),
        //    new Note("C2", "1/2", 120),
        //    new Note("B2", "1/2", 120),
        //    new Note("C2", "1/4", 120),
        //    new Note("C2", "1/4", 120),
        //    new Note("D2", "1/4", 120),
        //    new Note("E2", "1/4", 120),
        //    new Note("F2", "1/4", 120),
        //    new Note("E2", "1/4", 120),
        //    new Note("D2", "1/4", 120),
        //    new Note("C2", "1/4", 120),
        //    new Note("B2", "1/4", 120),
        //    new Note("G1", "1/4", 120),
        //    new Note("A2", "1/4", 120),
        //    new Note("B2", "1/4", 120),
        //    new Note("C2", "1/2", 120),
        //    new Note("C2", "1/2", 120)
        //};

    }


    /// <summary>
    /// This will play a sequence of notes on the lead instrument. 
    /// </summary>
    /// <param name="notes">Pass a queue of notes to be played</param>
    public void PlaySequence(Queue<Note> notes)
    {
        sequence = notes;

        if(lead.isPlaying != true)
        {
            playingSequence = true;
        }
    }


    /// <summary>
    /// Use this to set the key of the melody to be played
    /// </summary>
    /// <param name="rootValue">Please see Note definition for possible note values.</param>
    public void SetKey(string rootValue)
    {
        root = new Note(rootValue, "1/4", 120);
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

        //Scale Test Code: DELETE
        if(Input.GetKeyUp(KeyCode.W))
        {
            counter = System.Math.Min(counter + 1, possibleScales.Length - 1);
        }
        if(Input.GetKeyUp(KeyCode.S))
        {
            counter = System.Math.Max(counter - 1, 0);
        }
        if(Input.GetKeyUp(KeyCode.Space))
        {
            var scale = new Scale(possibleScales[counter], isMajor ? types[0] : types[1]).scale.ToArray();
            

            PlaySequence(new Queue<Note>(scale));
        }

        if(Input.GetKeyUp(KeyCode.X))
        {
            if (isMajor)
                isMajor = false;
            else
                isMajor = true;
        }


    }
}
