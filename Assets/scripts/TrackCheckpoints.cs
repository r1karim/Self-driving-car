using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackCheckpoints : MonoBehaviour
{
    public event EventHandler OnPlayerCorrectCheckpoint;
    public event EventHandler OnPlayerIncorrectCheckpoint;
    private List<CheckPoint> checkPointSingleList;
    private int nextCheckPointSingleIndex;
    private void Awake()
    {
        Transform checkpointsTransform = transform.Find("Checkpoints");
        checkPointSingleList = new List<CheckPoint>();
        foreach (Transform checkpoint in checkpointsTransform)
        {
            checkpoint.GetComponent<CheckPoint>().setTrackCheckpoints(this);
            checkPointSingleList.Add(checkpoint.GetComponent<CheckPoint>());
        }
        nextCheckPointSingleIndex = 0;
    }

    public void PlayerThroughCheckPoint(CheckPoint checkPointSingle)
    {
        if(nextCheckPointSingleIndex == checkPointSingleList.IndexOf(checkPointSingle))
        {
            nextCheckPointSingleIndex = (nextCheckPointSingleIndex + 1) % checkPointSingleList.Count;
            OnPlayerCorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnPlayerIncorrectCheckpoint?.Invoke(this, EventArgs.Empty);
        }
    }
    public void resetCheckpoints()
    {
        nextCheckPointSingleIndex = 0;
    }

    public CheckPoint getNextCheckpoint()
    {
        return checkPointSingleList[nextCheckPointSingleIndex];
    }
}
