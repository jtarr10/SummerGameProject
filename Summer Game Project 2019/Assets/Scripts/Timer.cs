using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //duration of the timer
    public float duration { get; set; }
    public bool isDone { get; protected set; }
    bool isCounting = false;
    float counter;


    /// <summary>
    /// Starts the timer with the passed float value as the timer duration.
    /// </summary>
    /// <param name="dur">This sets the timer duration in seconds.</param>
    public void StartTimer(float dur)
    {
        if(dur > 0.0f)
        {
            duration = dur;
            counter = duration;
            isCounting = true;
            //Debug.Log("Timer is starting with a duration of " + duration.ToString());
        }
        else
        {
            Debug.Log("ERROR: Counter set to a time less than or equal to 0 seconds");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(counter > 0.0f && isCounting == true)
        {
            counter -= Time.deltaTime;
            //Debug.Log(counter.ToString());
        }
        else if (counter <= 0.0f && isCounting == true)
        {
            isCounting = false;
            isDone = FinishCount();
            Debug.Log("Timer Finished");
        }
    }

    /// <summary>
    /// This function will return a boolean indicating whether or not the Timer is finished or not. If the timer is done, the component will be destroyed.
    /// </summary>
    /// <returns>true if finished and false if still counting</returns>
    public bool FinishCount()
    {
        if (counter > 0.0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
}
