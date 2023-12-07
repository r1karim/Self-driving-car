using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private TrackCheckpoints trackCheckpoints;
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CarController>(out CarController car))
        {
            trackCheckpoints.PlayerThroughCheckPoint(this);
        }
    }
    public  void setTrackCheckpoints(TrackCheckpoints trackCheckPoints)
    {
        this.trackCheckpoints = trackCheckPoints;
    }
}
