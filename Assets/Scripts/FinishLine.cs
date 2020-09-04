using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    public static Action OnLevelFinished;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            BroadcastOnPlayerFinished();
        }
    }

    private void BroadcastOnPlayerFinished()
    {
        if (OnLevelFinished != null)
            OnLevelFinished();
    }
}
