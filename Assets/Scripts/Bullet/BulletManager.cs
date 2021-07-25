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

    public Color playerColor;
    public Color AIColor;
    #endregion

    #region Private Variables
    private Rigidbody2D rb;
    private SpriteRenderer spRenderer;
    private bool m_isPlayer;
    #endregion

    #region Methods
    private void Awake()
    {
        //Cache Component in the begining
        rb = GetComponent<Rigidbody2D>();
        spRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Adds velocity to the bullet in the given direction
    /// </summary>
    /// <param name="direction">Direction of the bullet</param>
    /// <param name="isPlayerShot">Was this the player who shot the bullet?</param>
    public void ShootBullet(float direction, bool isPlayerShot)
    {
        m_isPlayer = isPlayerShot;
        rb.velocity = new Vector2(0, speed * direction);
        if (isPlayerShot)
            spRenderer.color = playerColor;
        else
            spRenderer.color = AIColor;
    }

    private void OnEnable()
    {
        Destroy(gameObject, 3);   //Destroy this object after x seconds
    }

    /// <summary>
    /// Handle collision with objects
    /// </summary>
    /// <param name="collision">The object that we hit</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_isPlayer)
        {
            if (collision.CompareTag("Enemy"))
            {
                GameManager.instance.AddScore(m_isPlayer);
                Destroy(gameObject);
            }
        }
        else
        {
            if (collision.CompareTag("Player"))
            {
                GameManager.instance.AddScore(m_isPlayer);
                Destroy(gameObject);
            }
        }
    }

    #endregion

}
