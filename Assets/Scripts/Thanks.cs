using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thanks : MonoBehaviour
{
    public float duration = 10;
    public float curTime = 0;
    public bool testThanks = false;
    Canvas can;
    private void Start()
    {
        can = GetComponent<Canvas>();
    }
    // Start is called before the first frame update
    public void StartThanks()
    {
        curTime = 0;
        can.enabled = true;
    }

    private void Update()
    {
        if(curTime >= duration)
        {
            
            can.enabled = false;
        }
        else
        {
            curTime += Time.deltaTime;
        }

        if(testThanks)
        {
            testThanks = false;
            StartThanks();
        }
    }
}
