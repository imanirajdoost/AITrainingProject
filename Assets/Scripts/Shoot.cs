using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manages shooting behaviour
/// </summary>
public class Shoot : MonoBehaviour
{
    //I can get my input for moving here but then
    //I cannot use this script for AI as well

    #region Public Variables
    public Transform shootingPosition;  //Where I start shooting (Tip of the gun)
    public GameObject bulletObject;
    #endregion

    #region Methods

    /// <summary>
    /// Sends the shoot command for the bullet in the given direction
    /// </summary>
    /// <param name="direction"></param>
    public void ShootBullet(float direction)
    {
        GameObject bulletRef = Instantiate(bulletObject, shootingPosition.position,Quaternion.identity);
        BulletManager bulletManager = bulletRef.GetComponent<BulletManager>();
        bulletManager.ShootBullet(direction);
    }

    private void OnEnable()
    {
        Destroy(this, 3);   //Destroy this object after x seconds
    }

    #endregion
}
