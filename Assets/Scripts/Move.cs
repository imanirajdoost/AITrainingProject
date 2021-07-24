using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages movement of the character
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class Move : MonoBehaviour
{
    //I can get my input for moving here but then
    //I cannot use this script for AI as well

    #region Public Variables
    public float speed = 1f;
    #endregion

    #region Private Variables
    private Rigidbody2D rb;
    #endregion

    #region Methods

    private void Awake()
    {
        //Cache the Componenets in the begining
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves the character to left or right
    /// </summary>
    /// <param name="direction">Right is positive and Left is negative, Zero is Stop</param>
    public void MoveCharacter(int direction)
    {
        int currentDir = 0;
        if (direction < 0)
            currentDir = -1;
        else if (direction > 0)
            currentDir = 1;
        rb.velocity = new Vector2(speed * currentDir, 0);
    }

    #endregion
}
