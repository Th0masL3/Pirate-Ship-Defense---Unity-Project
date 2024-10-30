using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int lives = 100;
    public int money = 50;
    public RoundData[] rounds;
    public int currentRound = 0;

    public int bloonsLeft = 0;

    private bool roundInProgress;
    private bool doAutoNextRound;
    
    public Track track;

    public int enemiesDefeated = 0;


    
    public void GameOver()
{
    if (lives <= 0)
    {
        SceneManager.LoadScene("GameOver");
    }
}


private IEnumerator spawnBloons()
    {
        RoundData round = rounds[currentRound];

        foreach (var data in round.bloons)
        {
            for (int i = 0; i < data.count; i++)
            {
                Bloon bloon = Instantiate(data.bloon, track.getWaypoint(0), quaternion.identity);
                bloonsLeft++;
                yield return new WaitForSeconds(data.spawnRate);
            }

            yield return new WaitForSeconds(round.timeBetweenBloons);
        }

        roundInProgress = false;
        money += round.money;
    }

    private IEnumerator autoNextRound()
    {
        while (true)
        {
            if (doAutoNextRound && bloonsLeft == 0 && !roundInProgress)
            {
                yield return new WaitForSeconds(2f);
                NextRound();
            }

            yield return null;
        }
    }
    

    public void NextRound()
    {
        if (roundInProgress || bloonsLeft > 0) return;
        roundInProgress = true;
        currentRound++;
        StartCoroutine(spawnBloons());
    }

    public void setAutoNextRound(bool v)
    {
        doAutoNextRound = v;
    }

    private void Awake()
    {
        instance = this;
        StartCoroutine(autoNextRound());
    }


    [System.Serializable]
    public class RoundData
    {
        public BloonData[] bloons;
        public float timeBetweenBloons = 0.6f;
        public int money = 10;
    }

    [Serializable]
    public class BloonData
    {
        public Bloon bloon;
        public int count = 1;
        public float spawnRate = 0.5f;
    }
}
