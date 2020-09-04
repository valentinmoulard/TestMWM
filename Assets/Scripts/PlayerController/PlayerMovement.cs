using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static Action<Vector3> OnPlayerBallStop;

    private const int MAX_DISTANCE_SWIPE = 200;

    [SerializeField]
    private float m_maxImpulseStrength = 0.0f;

    [SerializeField]
    private Rigidbody m_rigidbodyReference = null;

    [SerializeField]
    private Vector3 m_maxBallPositionOffsetWhenCharging = Vector3.zero;

    private Vector3 m_ballOriginalPositionBuffer;
    private Vector3 m_cursorStartPositionBuffer;
    private float m_implulseStrengthBuffer;
    float m_strengthFactor;
    private bool m_isBallStationary;

    private void OnEnable()
    {
        Controller.OnTapBegin += GetCursorStartPosition;
        Controller.OnRelease += MoveBall;
        Controller.OnHold += UpdateBallPositionBeforeLaunch;
        Obstacle.OnBallNotStopOnObstacle += StopBall;

        GameManager.OnNewLevelStarted += ResetPlayerState;
    }

    private void OnDisable()
    {
        Controller.OnTapBegin -= GetCursorStartPosition;
        Controller.OnRelease -= MoveBall;
        Controller.OnHold -= UpdateBallPositionBeforeLaunch;
        Obstacle.OnBallNotStopOnObstacle -= StopBall;

        GameManager.OnNewLevelStarted -= ResetPlayerState;
    }

    private void Start()
    {
        ResetPlayerState();
    }


    private void MoveBall(Vector3 cursorReleasePosition)
    {
        if (m_isBallStationary)
        {
            ComputeImpulseStrength(m_cursorStartPositionBuffer, cursorReleasePosition);
            if (m_implulseStrengthBuffer != 0)
            {
                m_rigidbodyReference.useGravity = true;
                m_isBallStationary = false;
                m_rigidbodyReference.AddForce(Vector3.up * m_implulseStrengthBuffer, ForceMode.Impulse);
            }
        }
        else
        {
            BroadcastOnPlayerBallStop(transform.position);
        }
    }


    private void GetCursorStartPosition(Vector3 cursorPosition)
    {
        if (m_isBallStationary)
        {
            m_cursorStartPositionBuffer = cursorPosition;
            m_ballOriginalPositionBuffer = transform.position;
        }
    }

    private void StopBall()
    {
        if (!m_isBallStationary)
        {
            m_rigidbodyReference.useGravity = false;
            m_isBallStationary = true;
            m_rigidbodyReference.velocity = Vector3.zero;
            BroadcastOnPlayerBallStop(transform.position);
        }
    }

    private void UpdateBallPositionBeforeLaunch(Vector3 cursorPosition)
    {
        if (m_isBallStationary)
        {
            ComputeImpulseStrength(m_cursorStartPositionBuffer, cursorPosition);
            Vector3 offset = m_maxBallPositionOffsetWhenCharging * m_strengthFactor;
            transform.position = m_ballOriginalPositionBuffer + offset;
        }
    }

    private void ComputeImpulseStrength(Vector3 cursorStartPosition, Vector3 cursorReleasePosition)
    {
        if (cursorStartPosition.y > cursorReleasePosition.y)
        {
            m_strengthFactor = Mathf.Clamp((Vector3.Distance(cursorStartPosition, cursorReleasePosition) / MAX_DISTANCE_SWIPE), 0, 1);
            m_implulseStrengthBuffer = m_maxImpulseStrength * m_strengthFactor;
        }
        else
        {
            m_strengthFactor = 0.0f;
            m_implulseStrengthBuffer = 0.0f;
        }
    }


    private void ResetPlayerState()
    {
        m_rigidbodyReference.useGravity = false;
        m_isBallStationary = true;
        m_rigidbodyReference.velocity = Vector3.zero;
    }


    private void BroadcastOnPlayerBallStop(Vector3 ballPosition)
    {
        if (OnPlayerBallStop != null)
            OnPlayerBallStop(ballPosition);
    }
}
