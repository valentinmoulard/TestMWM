using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusTarget : MonoBehaviour
{
    public static Action<int> OnPlayerStopOnBonus;

    [SerializeField]
    private List<int> m_bonusPointRange = null;

    [SerializeField]
    private float m_bonusRadius = 0.0f;

    private int m_pointRangeCount;
    private float m_bonusRangeStep;

    private void OnEnable()
    {
        PlayerMovement.OnPlayerBallStop += CheckPlayerBallOnBonus;
    }

    private void OnDisable()
    {
        PlayerMovement.OnPlayerBallStop -= CheckPlayerBallOnBonus;
    }


    void Start()
    {
        if (m_bonusPointRange == null || m_bonusPointRange.Count == 0)
            Debug.LogError("The bonus list is empty for this object!", gameObject);

        m_pointRangeCount = m_bonusPointRange.Count;
        m_bonusRangeStep = m_bonusRadius / m_pointRangeCount;
        transform.localScale = new Vector3(m_bonusRadius, 0.2f, m_bonusRadius);
    }


    private void CheckPlayerBallOnBonus(Vector3 playerPosition)
    {
        int scoreIndex = GetBonusPoints(playerPosition);

        if (scoreIndex > -1)
        {
            BroadcastOnPlayerStopOnBonus(m_bonusPointRange[scoreIndex]);
            Destroy(gameObject);
        }
    }

    private int GetBonusPoints(Vector3 playerPosition)
    {
        for (int i = 0; i < m_pointRangeCount; i++)
        {
            if (IsPlayerInRange(playerPosition, (m_bonusRangeStep * (i + 1)) / 2))
                return i;
        }

        return -1;
    }

    private bool IsPlayerInRange(Vector3 playerPosition, float range)
    {
        if (playerPosition.y < transform.position.y + range && playerPosition.y > transform.position.y - range)
            return true;

        return false;
    }

    private void BroadcastOnPlayerStopOnBonus(int points)
    {
        if (OnPlayerStopOnBonus != null)
            OnPlayerStopOnBonus(points);
    }
}
