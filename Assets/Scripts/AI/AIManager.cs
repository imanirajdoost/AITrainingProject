using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    #region Private Variables

    private Move moveManager;
    private Shoot shootManager;

    [Range(0.01f,1f)]
    public float decisionTime = 0.2f;
    public AIType currentAIType = AIType.Random;
    public States currentState = States.MoveLeft;
    #endregion

    #region Enums
    public enum AIType { Random, Greedy, Smart }
    public enum States { MoveRight, MoveLeft, Shoot }
    #endregion

    #region Methods

    private void Awake()
    {
        //Cache Componenets in the begining
        moveManager = GetComponent<Move>();
        shootManager = GetComponent<Shoot>();
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

    }

    private void RunSmart()
    {

    }

    #endregion
}