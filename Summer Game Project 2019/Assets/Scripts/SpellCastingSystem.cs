using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellCastingSystem : MonoBehaviour
{



    //Singleton Setup
    public static SpellCastingSystem Instance { get; private set; }
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
