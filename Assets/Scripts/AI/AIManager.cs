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
        
    }

    #endregion
}