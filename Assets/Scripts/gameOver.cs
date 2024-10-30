using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gameOver : MonoBehaviour
{

    int EnemiesDefeated = GameManager.instance.enemiesDefeated;

    [SerializeField] public TMP_Text enemiesText;

    void Start()
    {
        // Retrieve and display the number of enemies defeated
        enemiesText.text = "Enemies Defeated: " + GameManager.instance.enemiesDefeated.ToString();
        
    }


    public void LoadGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}