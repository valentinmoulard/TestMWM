using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public static Action OnBallStopOnObstacle;
    public static Action OnBallNotStopOnObstacle;
    private float m_obstacleHeight;


    private void OnEnable()
    {
        PlayerMovement.OnPlayerBallStop += CheckPlayerBallOnObstacle;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerBallStop -= CheckPlayerBallOnObstacle;
    }

    void Start()
    {
        m_obstacleHeight = transform.localScale.y;
    }

    private void CheckPlayerBallOnObstacle(Vector3 playerPosition)
    {
        if (playerPosition.y < transform.position.y + m_obstacleHeight/2 && playerPosition.y > transform.position.y - m_obstacleHeight / 2)
        {
            BroadcastOnBallStopOnObstacle();
        }
        else
        {
            BroadcastOnBallNotStopOnObstacle();
        }
    }

    private void BroadcastOnBallStopOnObstacle()
    {
        if (OnBallStopOnObstacle != null)
            OnBallStopOnObstacle();
    }

    private void BroadcastOnBallNotStopOnObstacle()
    {
        if (OnBallNotStopOnObstacle != null)
            OnBallNotStopOnObstacle();
    }
}
