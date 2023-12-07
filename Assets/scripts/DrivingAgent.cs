using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using System;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.VisualScripting;

public class DrivingAgent : Agent
{
    [SerializeField] private TrackCheckpoints trackCheckpoints;
    [SerializeField] private Transform spawn;
    private CarController carController;
    private Vector3 oldPosition;

    private void Awake()
    {
        carController = GetComponent<CarController>();
    }
    private void Start()
    {
        trackCheckpoints.OnPlayerCorrectCheckpoint += TrackCheckpoints_OnCarCorrectCheckpoint;
        trackCheckpoints.OnPlayerIncorrectCheckpoint += TrackCheckpoints_OnCarIncorrectCheckpoint;
    }
    private void TrackCheckpoints_OnCarCorrectCheckpoint(object sender, EventArgs e)
    {
        AddReward(1f);
    }
    private void TrackCheckpoints_OnCarIncorrectCheckpoint(object sender, EventArgs e)
    {
        AddReward(-1f);
    }

    public override void OnEpisodeBegin()
    {
        oldPosition = spawn.position;
        transform.position = spawn.position;
        transform.forward = spawn.forward;
        trackCheckpoints.resetCheckpoints();
        carController.stop();
    }
    public override void CollectObservations(VectorSensor sensor)
    {
        Vector3 checkpointForward = trackCheckpoints.getNextCheckpoint().transform.forward;
        float directionDot = Vector3.Dot(transform.forward, checkpointForward);
        sensor.AddObservation(directionDot);
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        float forwardAmount = 0f;
        float turnAmount = 0f;

        switch(actions.DiscreteActions[0])
        {
            case 0:forwardAmount = 0; break;
            case 1:forwardAmount += 1f; break;
            case 2: forwardAmount -= 1f; break;

        }
        switch (actions.DiscreteActions[1])
        {
            case 0: turnAmount = 0; break;
            case 1: turnAmount += 1f; break;
            case 2: turnAmount -= 1f; break;
        }
        carController.setInput(forwardAmount, turnAmount);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        int forwardAction = 0;
        if (Input.GetAxis("Vertical") > 0) forwardAction = 1;
        if (Input.GetAxis("Vertical") < 0) forwardAction = 2;

        int turnAction = 0;
        if (Input.GetAxis("Horizontal") > 0) turnAction = 1;
        if (Input.GetAxis("Horizontal") < 0) turnAction = 2;
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        discreteActions[0] = forwardAction;
        discreteActions[1]=turnAction;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            AddReward(-0.5f);
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Obstacle>(out Obstacle obstacle))
        {
            AddReward(-0.1f);
        }
    }
    private void Update()
    {
        if ( (oldPosition[1] - transform.position[1]) > 4)
        {
            AddReward(-1f);
            EndEpisode();
        }
    }
}
