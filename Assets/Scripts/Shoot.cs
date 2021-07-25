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
    public float shootDelay = 0.2f;
    #endregion

    #region Private Variables
    private bool canShoot = true;
    private Coroutine shootDelayCoroutine;
    #endregion

    #region Methods

    /// <summary>
    /// Sends the shoot command for the bullet in the given direction
    /// </summary>
    /// <param name="direction">Direction of the bullet</param>
    /// <param name="isPlayer">Was this the player who shot the bullet?</param>
    public void ShootBullet(float direction, bool isPlayer)
    {
        if (canShoot)
        {
            GameObject bulletRef = Instantiate(bulletObject, shootingPosition.position, Quaternion.identity);
            BulletManager bulletManager = bulletRef.GetComponent<BulletManager>();
            bulletManager.ShootBullet(direction, isPlayer);

            if (shootDelayCoroutine != null)
                StopCoroutine(shootDelayCoroutine);
            shootDelayCoroutine = StartCoroutine(DisableShooting());
        }
    }

    /// <summary>
    /// Disables shooting for x amount of seconds
    /// </summary>
    /// <returns></returns>
    private IEnumerator DisableShooting()
    {
        canShoot = false;
        yield return new WaitForSeconds(shootDelay);
        canShoot = true;
    }

    /// <summary>
    /// Returns if character can shoot or not
    /// </summary>
    /// <returns>Can shoot</returns>
    public bool CanShoot()
    {
        return canShoot;
    }

    #endregion
}
