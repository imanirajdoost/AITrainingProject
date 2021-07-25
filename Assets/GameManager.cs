using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// This class manages everything in the game
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Public Variables
    public static GameManager instance;

    public TextMeshProUGUI playerScore;
    public TextMeshProUGUI enemyScore;
    #endregion

    #region Private Variables

    private int playerCurrentScore = 0;
    private int enemyCurrentScore = 0;
    #endregion

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Adds a point to the player or the enemy as they get hit
    /// </summary>
    /// <param name="isPlayer">Is it the player who gets the point?</param>
    public void AddScore(bool isPlayer)
    {
        if (isPlayer)
            playerCurrentScore++;
        else
            enemyCurrentScore++;
        UpdateUI();
    }

    /// <summary>
    /// Updates our score table UI
    /// </summary>
    private void UpdateUI()
    {
        playerScore.text = playerCurrentScore.ToString();
        enemyScore.text = enemyCurrentScore.ToString();
    }
}
