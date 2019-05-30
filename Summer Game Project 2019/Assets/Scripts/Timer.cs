using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    //duration of the timer
    public float duration { get; set; }
    public bool isDone { get; protected set; }
    bool isCounting = false;
    public float count;


    /// <summary>
    /// Starts the timer with the passed float value as the timer duration.
    /// </summary>
    /// <param name="dur">This sets the timer duration in seconds.</param>
    public void StartTimer(float dur)
    {
        if(dur > 0.0f)
        {
            duration = dur;
            count = duration;
            isCounting = true;
            //Debug.Log("Timer is starting with a duration of " + duration.ToString());
        }
        else
        {
            Debug.Log("ERROR: count set to a time less than or equal to 0 seconds");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(count > 0.0f && isCounting == true)
        {
            count -= Time.deltaTime;
            //Debug.Log(count.ToString());
        }
        else if (count <= 0.0f && isCounting == true)
        {
            Debug.Log("Timer Finished");
            isCounting = false;
            isDone = FinishCount();
        }
    }

    /// <summary>
    /// This function will return a boolean indicating whether or not the Timer is finished or not. If the timer is done, the component will be destroyed.
    /// </summary>
    /// <returns>true if finished and false if still counting</returns>
    public bool FinishCount()
    {
        if (count > 0.0f)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    
}
