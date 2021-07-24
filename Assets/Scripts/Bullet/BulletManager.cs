using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages bullet behaviour
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BulletManager : MonoBehaviour
{
    #region Public Variables

    public float speed = 2f;
    #endregion

    #region Private Variables
    private Rigidbody2D rb;
    #endregion

    #region Methods
    private void Awake()
    {
        //Cache Component in the begining
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Adds velocity to the bullet in the given direction
    /// </summary>
    /// <param name="direction">Direction of the bullet</param>
    public void ShootBullet(float direction)
    {
        rb.velocity = new Vector2(0, speed * direction);
    }

    #endregion

}
