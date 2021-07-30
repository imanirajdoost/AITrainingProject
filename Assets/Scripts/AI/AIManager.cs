using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    #region Private Variables

    private Move moveManager;
    private Shoot shootManager;
    private CircleCollider2D col;
    private List<CircleCollider2D> availablePoints;
    #endregion

    #region Public Variables

    [Range(0.01f,1f)]
    public float decisionTime = 0.2f;
    public AIType currentAIType = AIType.Random;
    public States currentState = States.MoveLeft;
    public float playerOffset = 0.05f;
    public LayerMask whatToDetect;
    public float xLimit = 0.85f;
    public int maxNumberOfLoops = 15;
    private bool isMoving = false;
    public GameObject pointPrefab;
    private CircleCollider2D nextSafePosition;
    private List<CircleCollider2D> safePositions;
    #endregion

    #region Enums
    public enum AIType { Random, Greedy, Smart }
    public enum States { MoveRight, MoveLeft, Shoot,GoToSafety }
    #endregion

    #region Methods

    private void Awake()
    {
        //Cache Componenets in the begining
        moveManager = GetComponent<Move>();
        shootManager = GetComponent<Shoot>();
        col = GetComponent<CircleCollider2D>();

        CreateAllPoints();
        nextSafePosition = availablePoints[0];
        safePositions = new List<CircleCollider2D>();
    }

    /// <summary>
    /// Creates all the Vector2 positions that AI can move to
    /// </summary>
    private void CreateAllPoints()
    {
        //Start From the left
        availablePoints = new List<CircleCollider2D>();
        Vector2 currentPos = new Vector2(-xLimit,transform.position.y);
        while (currentPos.x < xLimit)
        {
            GameObject g = Instantiate(pointPrefab,currentPos,Quaternion.identity);
            availablePoints.Add(g.GetComponent<CircleCollider2D>());
            currentPos.x += col.radius;
        }
    }

    private void Start()
    {
        //Run the AI decision maker accordingly
        switch (currentAIType)
        {
            case AIType.Random:
                InvokeRepeating("RunRandom",0,decisionTime);
                break;
            case AIType.Greedy:
                InvokeRepeating("RunGreedy", 0, decisionTime);
                break;
            case AIType.Smart:
                InvokeRepeating("RunSmart", 0, decisionTime);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Finds all the enemy's (Player) bullets that are coming towards the AI
    /// </summary>
    /// <returns></returns>
    private BulletManager[] FindAllDangerousBullets()
    {
        BulletManager[] allBullets = FindObjectsOfType<BulletManager>();
        if (allBullets != null)
        {
            List<BulletManager> playerBullets = new List<BulletManager>();
            for (int i = 0; i < allBullets.Length; i++)
            {
                if (allBullets[i].IsPlayersBullet())
                {
                    //If it's above me, I don't care about it, otherwise it's dangerous
                    if(allBullets[i].transform.position.y < (transform.position.y + playerOffset))
                        playerBullets.Add(allBullets[i]);
                }
            }
            return playerBullets.ToArray();
        }
        return null;
    }

    /// <summary>
    /// Looks for the nearest safe zone on the X Axis along the X limit of movement
    /// It starts by looking around the nearest neighbors on the right or left
    /// </summary>
    private Vector2 LookForNextSafeZone()
    {
        //Our threshold will be the radius of AI's collider
        safePositions = new List<CircleCollider2D>();

        BulletManager[] bullets = FindAllDangerousBullets();

        if (bullets != null && bullets.Length > 0)
        {
            Debug.Log("Found Bullets");
            for (int i = 0; i < bullets.Length; i++)
            {
                Collider2D[] dangerZones = Physics2D.OverlapCircleAll(
                    new Vector2(bullets[i].transform.position.x, transform.position.y),
                    shootManager.GetBulletRadius() + 0.1f,
                    whatToDetect);

                //If we have danger zones, it means we have to avoid them
                if (dangerZones != null && dangerZones.Length > 0)
                {
                    //Safe positions = Available Points - Danger Zones
                    for (int j = 0; j < availablePoints.Count; j++)
                    {
                        bool isSafe = true;
                        for (int k = 0; k < dangerZones.Length; k++)
                        {
                            if (availablePoints[j].gameObject == dangerZones[k].gameObject)
                                isSafe = false;
                        }
                        if (isSafe)
                            safePositions.Add(availablePoints[j]);
                    }
                }
            }
        }
        //If our next safe position is among the safe positions that we found, we will continue going that way
        for (int i = 0; i < safePositions.Count; i++)
        {
            if (safePositions[i] == nextSafePosition)
                return nextSafePosition.transform.position;
        }
        //If it's no longer safe, then update the safe position
        int randIndex = UnityEngine.Random.Range(0, safePositions.Count);

        if (randIndex >= 0 && safePositions.Count > 0)
            nextSafePosition = safePositions[randIndex];
        else
            nextSafePosition = availablePoints[20];

        return nextSafePosition.transform.position;
    }

    private void OnDrawGizmos()
    {
        if (availablePoints != null && availablePoints.Count > 0)
        {
            for (int i = 0; i < availablePoints.Count; i++)
                Gizmos.DrawWireSphere(availablePoints[i].transform.position, col.radius);
        }
    }

    /// <summary>
    /// Moves the AI to the given position
    /// </summary>
    /// <param name="pos">Position to move towards</param>
    private void MoveToPosition(Vector2 pos)
    {
        if (transform.position.x < pos.x + 0.02f && transform.position.x > pos.x - 0.02f)
        {
            moveManager.MoveCharacter(0);       //Stay right there!
            isMoving = false;
        }
        else if (pos.x > transform.position.x)
            moveManager.MoveCharacter(1);
        else if (pos.x < transform.position.x)
            moveManager.MoveCharacter(-1);
    }

    private void Update()
    {
        switch (currentState)
        {
            case States.MoveRight:
                moveManager.MoveCharacter(1);
                break;
            case States.MoveLeft:
                moveManager.MoveCharacter(-1);
                break;
            case States.Shoot:
                shootManager.ShootBullet(-1, false);
                break;
            case States.GoToSafety:
                isMoving = true;
                MoveToPosition(LookForNextSafeZone());
                break;
            default:
                break;
        }
    }

    private void RunRandom()
    {
        Debug.Log("Running Random Decision...");
        //Get All the possible Actions
        int statesCount = Enum.GetNames(typeof(States)).Length;
        //Choose one of the actions COMPLETELY Randomly
        int decision = UnityEngine.Random.Range(0,statesCount);
        //Apply the Action
        currentState = (States)decision;

        Debug.Log("Finished Taking Decision.");
    }

    private void RunGreedy()
    {
        Debug.Log("Running Greedy Decision...");
        //Get All the possible Actions
        int statesCount = Enum.GetNames(typeof(States)).Length;
        //Always do the action that you think is best!
        int chance = UnityEngine.Random.Range(0,2);
        if (chance == 0)
            currentState = States.GoToSafety;
        else
            currentState = States.Shoot;
    }

    private void RunSmart()
    {

    }

    #endregion
}