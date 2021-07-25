using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages all behaviours of our Player
/// Which is controlled by a human
/// </summary>
public class PlayerManager : MonoBehaviour
{
    #region Private Variables
    private Shoot shootManager;
    private Move moveManager;
    #endregion

    #region Methods

    private void Awake()
    {
        //Cache Componenets in the begining
        moveManager = GetComponent<Move>();
        shootManager = GetComponent<Shoot>();
    }

    private void Update()
    {
        //Get Inputs

        #region Movement

        float dir = Input.GetAxisRaw("Horizontal");
        moveManager.MoveCharacter((int)dir);    //Move the player

        #endregion

        #region Shooting

        if (Input.GetKeyDown(KeyCode.Space))
            shootManager.ShootBullet(1,true);

        #endregion
    }
    #endregion
}
