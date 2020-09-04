using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_scoreTexteReference = null;

    private int m_currentScore;

    private void OnEnable()
    {
        BonusTarget.OnPlayerStopOnBonus += IncreaseScore;
    }

    private void OnDisable()
    {
        BonusTarget.OnPlayerStopOnBonus -= IncreaseScore;
    }

    private void Start()
    {
        if (m_scoreTexteReference == null)
            Debug.LogError("No text component attached in the inspector!", gameObject);

        m_currentScore = 0;
        UpdateScoreUI();
    }

    private void IncreaseScore(int amount)
    {
        m_currentScore += amount;
        UpdateScoreUI();
    }

    private void UpdateScoreUI()
    {
        m_scoreTexteReference.text = m_currentScore.ToString();
    }
}
