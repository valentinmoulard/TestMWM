using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static Action OnBallStopOnObstacle;
    public static Action OnBallNotStopOnObstacle;
    private float m_obstacleHeight;
    [SerializeField]
    private List<GameObject> m_obstacleList;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerBallStop += CheckPlayerBallOnObstacle;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerBallStop -= CheckPlayerBallOnObstacle;
    }

    private void CheckPlayerBallOnObstacle(Vector3 playerPosition)
    {
        for (int i = 0; i < m_obstacleList.Count; i++)
        {
            m_obstacleHeight = m_obstacleList[i].gameObject.transform.localScale.y;
            if (playerPosition.y < m_obstacleList[i].gameObject.transform.position.y + m_obstacleHeight / 2 && playerPosition.y > m_obstacleList[i].gameObject.transform.position.y - m_obstacleHeight / 2)
            {
                BroadcastOnBallStopOnObstacle();
                return;
            }
        }

        BroadcastOnBallNotStopOnObstacle();
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
