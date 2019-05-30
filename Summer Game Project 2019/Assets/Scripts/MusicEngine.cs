using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class HelperFunctions
{
    /// <summary>
    /// This function limits a value within a minimum and maximum value.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="inclusiveMinimum"></param>
    /// <param name="inclusiveMaximum"></param>
    /// <returns></returns>
    public static int LimitToRange(this int value, int inclusiveMinimum, int inclusiveMaximum)
    {
        if (value < inclusiveMinimum) { return inclusiveMinimum; }
        if (value > inclusiveMaximum) { return inclusiveMaximum; }
        return value;
    }
}


public struct Note
{
    public string value;
    public string type;
    public float duration;
    public float freq;
    public int bpm;
    Dictionary<string, float> freqPairs;
    public Dictionary<string, float> typeDurations;

    /// <summary>
    /// This will initialize a note object with the given note value and type. The available note values are as follows:
    /// A1, A1#, B1, C1, C1#, D1, D1#, E1, F1, F1#, G1, G1#, A2, A2#, B2, C2, C2#, D2, D2#, E2, F2, F2#, G2, G2#, A3, A3#, B3, C3. If no note should be played,
    /// the user can pass the word "rest" as the noteValue. 
    /// The possible note types are as follows: 1/32, 1/16, 1/8, 1/6, 1/4, 1/3, dotted-1/4, 1/2, dotted-half, whole, 2-whole. The 1/4 note gets the beat as by convention of 4/4 time.
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
            ["1/6"] = beatLength / 1.5f,
            ["1/4"] = beatLength,
            ["1/3"] = beatLength * (4f / 3f),
            ["dotted-1/4"] = beatLength * 1.5f,
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
    /// A scale generates a list of notes that form up to two octaves of a specified scale type based from the root. 
    /// Scale.scale is the list of notes
    /// </summary>
    /// <param name="rootNote">This is the root note of the scale. </param>
    /// <param name="type">The scale can be either major or minor. Pass one of the two strings to the type parameter.</param>
    public Scale(string rootNote, string type)
    {
        root = rootNote;

        //setting up the note values of the two octaves of chromatic scale
        noteValues = new string[] { "C1", "C1#", "D1", "D1#", "E1", "F1", "F1#", "G1", "G1#", "A2", "A2#", "B2", "C2", "C2#", "D2", "D2#", "E2", "F2", "F2#", "G2", "G2#", "A3", "A3#", "B3", "C3" };

        //setting up step patterns
        stepPatterns = new Dictionary<string, string>
        {
            ["major"] = "WWHWWWHWWHWWWH",
            ["minor"] = "WHWWHWWWHWWHWW"
        };

        scale = new List<Note>();
        scale.Add(new Note(root, "1/4", 120));

        var notePointer = System.Array.IndexOf(noteValues, root);
        foreach(char letter in stepPatterns[type])
        {

            if (letter == 'W' && notePointer < noteValues.Length - 2)
            {
                notePointer += 2;
            }
            else if (letter == 'H' && notePointer < noteValues.Length - 1)
            {
                notePointer += 1;
            }
            else
                break;

            scale.Add(new Note(noteValues[notePointer], "1/4", 120));
        }
    }


}


public struct RandomMelody
{
    public string root;
    public List<Note> melody;
    public int bpm;
    Note[] key;
    float length;


    //Probability attributes
    float restProb;
    float disjunctivity;
    

    /// <summary>
    /// This struct will generate a string of notes that total to the given length, in the given key, following with given probabilities, and with the given bpm.
    /// This melody is produced as a list of notes called Melody.melody.
    /// </summary>
    /// <param name="rootKey">The root note of the key that the melody will be in.</param>
    /// <param name="probabilityOfRest">Probability that rests will be used in place of notes. (0 to 1)</param>
    /// <param name="disjunct">A parameter that controls how disjunct the melody will be. (values > 1 will increase the intervals)</param>
    /// <param name="typeVariation">A parameter that controls how varied the note durations will be. </param>
    /// <param name="rate">This is the bpm of the desired melody.</param>
    /// <param name="lengthInBeats">This is the length of the desired melody in beats.</param>
    public RandomMelody(string rootKey, float probabilityOfRest, float disjunct, int rate, int lengthInBeats)
    {
        //probability attributes
        restProb = probabilityOfRest;
        disjunctivity = System.Math.Max(0, disjunct);

        //melody attributes
        melody = new List<Note>();
        root = rootKey;
        bpm = rate;
        length = (60.0f / (float)bpm) * (float)lengthInBeats;

        //setting up the key
        key = new Scale(root, "major").scale.ToArray();

        string[] types = new string[] {"1/16", "1/8", "1/4", "1/3", "1/6" };
        int[] endNotes = new int[] { 0, 2, 4, 7, 9, 11 };
        int typeIndex = (int)Random.Range(0, types.Length - 1);
        int noteIndex = (int)Random.Range(0, key.Length - 1);
        

        float durTotal = 0;

        //This variable houses the next randomly generated note to be added to the melody
        Note next = new Note(key[noteIndex].value, types[typeIndex], bpm);
        

        while(durTotal < length)
        {
            //add the note and update the duration total
            melody.Add(next);
            durTotal += next.typeDurations[types[typeIndex]];



            noteIndex = HelperFunctions.LimitToRange(noteIndex + (int)Random.Range(-1.0f * disjunctivity, 1.0f * disjunctivity), 0, key.Length - 1);
            typeIndex = Random.Range(0, types.Length - 1);
               
            //dynamic adaptation of probability 
            if (next.value == key[noteIndex].value)
                disjunctivity += 1f;
            else
                disjunctivity -= 0.5f;

                  

            //dealing with rest probability
            var isRest = Random.Range(1f, 100f) <= (restProb * 100f);
            string[] restType = new string[] { "1/8", "1/4" };

            //catching the end of the pattern
            if (durTotal < next.typeDurations["1/3"])
            {
                if (durTotal == next.typeDurations["1/3"])
                    typeIndex = 3;
                else if (durTotal == next.typeDurations["1/4"])
                    typeIndex = 2;
                else if (durTotal == next.typeDurations["1/6"])
                    typeIndex = 4;
                else if (durTotal == next.typeDurations["1/8"])
                    typeIndex = 1;
                else
                {
                    typeIndex = 0;
                    isRest = true;
                }

                //finding the closest final note
                var temp = 0;
                foreach(int index in endNotes)
                {
                    if (System.Math.Abs(index - noteIndex) < System.Math.Abs(index - temp))
                        temp = index;
                }
                noteIndex = temp;

            }

            //setting up the next note to be added
            if (isRest)
            {
                next = new Note("rest", restType[(int)Random.Range(0, 1)], bpm);
            }
            else
            {
                next = new Note(key[noteIndex].value, types[typeIndex], bpm);
            }

            


        }


    }

}


public struct Spell
{
    Dictionary<string, string[]> castingComponents, songComponents;
    string genre;
    GameObject baseSpellObj, upgSpellObj, rightPermutationObj, leftPermutationObj;
    public int level;
    public string alignment;

    /// <summary>
    /// This will instantiate a Spell object with the following parameters.
    /// </summary>
    /// <param name="leadName">The name of the lead sound file.</param>
    /// <param name="bassName">The name of the bass sound file.</param>
    /// <param name="drumName">The name of the drum sound files (left index 0, right index 1)</param>
    /// <param name="genreName">The name of the spell genre.</param>
    /// <param name="baseSpell">The name of the spell GameObject prefab.</param>
    /// <param name="upgrade">The name of the upgraded spell GameObject prefab.</param>
    /// <param name="right">The name of the right drum track permutation GameObject prefab.</param>
    /// <param name="left">The name of the left drum track permutation GameObject prefab.</param>
    public Spell(string[] leadSnippets, string bassIntro, string[] drumIntros, string leadName, string bassName, string[] drumNames, string genreName, string baseSpell, string upgrade, string right, string left)
    {
        //setting up the components to each song piece 
        castingComponents = new Dictionary<string, string[]>
        {
            ["lead"] = leadSnippets,
            ["bass"] = new string[] { bassIntro },
            ["drums"] = drumIntros
        };

        songComponents = new Dictionary<string, string[]>
        {
            ["lead"] = new string[] { leadName },
            ["bass"] = new string[] { bassName },
            ["drums"] = drumNames
        };


        genre = genreName;

        baseSpellObj = Resources.Load<GameObject>("Prefabs\\" + baseSpell);
        upgSpellObj = Resources.Load<GameObject>("Prefabs\\" + upgrade);
        rightPermutationObj = Resources.Load<GameObject>("Prefabs\\" + right);
        leftPermutationObj = Resources.Load<GameObject>("Prefabs\\" + left);
        level = 1;
        alignment = "None";
    }

    /// <summary>
    /// returns the songs genre
    /// </summary>
    /// <returns></returns>
    public string GetGenre()
    {
        return genre;
    }

    /// <summary>
    /// called when the spell is being leveled up
    /// </summary>
    public void LevelUp(string align = "None")
    {
        if (level == 1)
            level++;
        else if (level == 2)
        {
            level++;
            alignment = align;
        }
        else
            Debug.Log("The spell is at max level. Could not level up any more");
    }

}



public class MusicEngine : MonoBehaviour
{
    public Note root;
    Queue<Note> sequence;
    Instrument lead, bass, drums;
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
    // Variable initialization / First time setup
    void Start()
    {
        playingSequence = false;
        lead = GameObject.Find("Lead").GetComponent<Instrument>();
        bass = GameObject.Find("Middle Layer").GetComponent<Instrument>();
        drums = GameObject.Find("Drum Layer").GetComponent<Instrument>();

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
    /// Plays a triple layered segment of music. The default will only require a lead clip to play.
    /// If the bassName or drumName parameters are set, they will accompany the lead part during playback.
    /// </summary>
    /// <param name="leadName">The name of the audio file in the Resources\Audio directory without extension.</param>
    /// <param name="bassName">An optional parameter for a filename of the bass line/ chords.</param>
    /// <param name="drumName">An optional parameter for a filename of the drum track.</param>
    public void PlayLayeredSegment(string leadName, string bassName = "None", string drumName = "None")
    {
        //Playback Setup
        lead.SetSound(leadName);
        if (bassName != "None")
            bass.SetSound(bassName);
        if (drumName != "None")
            drums.SetSound(drumName);

        //Trigger playback
        lead.Play();
        if (bassName != "None")
            bass.Play();
        if (drumName != "None")
            drums.Play();
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


    }
}
