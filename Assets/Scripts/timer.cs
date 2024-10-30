using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class timer : MonoBehaviour
{
    private float time;
    [SerializeField] private TMP_Text _timerText;

    void Start()
    {
        time = 0;
    }

    void Update()
    {
        
        time += Time.deltaTime;
        int timeInSeconds = Mathf.RoundToInt(time);
        _timerText.text = " Time: " + timeInSeconds.ToString();
        
    }



    public float getTime()
    {
        return time;
    }
}