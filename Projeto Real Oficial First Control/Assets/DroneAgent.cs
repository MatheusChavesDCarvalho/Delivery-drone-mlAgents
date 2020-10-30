using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Net;

public class DroneAgent : Agent
{
    Rigidbody rBody;
    
    float Forward = 0;
    float FlyLeft = 0;
    float FlyRight = 0;
    float Backward = 0;
    float Up = 0;
    float Down = 0;
    float TiltRight = 0;
    float TiltLeft = 0;
    float distanceToTargetBefore;


    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        
        
    }

    

    public Transform Target;
    public override void OnEpisodeBegin()
    {
        

            // If the Agent fell, zero its momentum
            this.rBody.angularVelocity = Vector3.zero;
            this.rBody.velocity = Vector3.zero;
            this.transform.localPosition = new Vector3(0, 5f, 0);
            this.transform.localRotation = new Quaternion(0, 0, 0, 1);


        // Move the target to a new spot
        int pos = Random.Range(0, 16);
        switch (pos)
        {
            case 0:
                Target.localPosition = new Vector3(0.4f, 12.5f, -265.2f);
                break;
            case 1:
                Target.localPosition = new Vector3(173.9f, 124.4f, -132.2f);
                break;
            case 2:
                Target.localPosition = new Vector3(189.5f, 170.5f, 186.7f);
                break;
            case 3:
                Target.localPosition = new Vector3(-56f, 75.8f, 124.6f);
                break;
            case 4:
                Target.localPosition = new Vector3(-213.3f, 85.9f, 53.2f);
                break;
            case 5:
                Target.localPosition = new Vector3(-162.8f, 11.4f, -44.4f);
                break;
            case 6:
                Target.localPosition = new Vector3(-95.2f, 68.25f, 2.81f);
                break;
            case 7:
                Target.localPosition = new Vector3(268.6f, 99.6f, -173.1f);
                break;
            case 8:
                Target.localPosition = new Vector3(202.7f, 13.2f, -201.9f);
                break;
            case 9:
                Target.localPosition = new Vector3(-62.1f, 80.3f, -233.7f) ;
                break;
            case 10:
                Target.localPosition = new Vector3(-132.49f, 80.3f, -133.41f);
                break;
            case 11:
                Target.localPosition = new Vector3(-212.6f, 39f, -47.9f);
                break;
            case 12:
                Target.localPosition = new Vector3(137.5f, 136.2f, 167.82f);
                break;
            case 13:
                Target.localPosition = new Vector3(106.2f, 11f, -101.9f);
                break;
            case 14:
                Target.localPosition = new Vector3(-231.1f, 120.2f, -271.1f);
                break;
            case 15:
                Target.localPosition = new Vector3(-184.8f, 26.7f, -180.1f);
                break;

        }
        //Target.localPosition = new Vector3(Random.Range(-280,280),
        //                                   12f,
        //                                   Random.Range(-280,280));
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(Target.localPosition);
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity);
    }

    public float ForceMod = 10;
    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(-0.01f);

        // Actions, size = 8
        
        Forward = vectorAction[0] * ForceMod;
        FlyLeft = vectorAction[1] * ForceMod;
        FlyRight = vectorAction[2] * ForceMod;
        Backward = vectorAction[3] * ForceMod;
        Up = vectorAction[4] * ForceMod;
        Down = vectorAction[5] * ForceMod;
        TiltRight = vectorAction[6] * ForceMod; 
        TiltLeft = vectorAction[7] * ForceMod;
        

        // Rewards
        float distanceToTargetNow = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTargetNow < distanceToTargetBefore)
        {
            SetReward(0.005f);
        }
        else
        {
            AddReward(-0.01f);
        }

        distanceToTargetBefore = distanceToTargetNow;



        //Drone Controls
        
            { rBody.AddRelativeTorque(0, TiltLeft / -1, 0); }//tilt drone left

            { rBody.AddRelativeTorque(0, TiltRight, 0); }//tilt drone right

            { rBody.AddRelativeForce(0, 0, Forward); rBody.AddRelativeTorque(Forward/5f, 0, 0); }//drone fly forward

            { rBody.AddRelativeForce(FlyLeft / -1, 0, 0); rBody.AddRelativeTorque(0, 0, FlyLeft/5f); }//rotate drone left

            { rBody.AddRelativeForce(FlyRight, 0, 0); rBody.AddRelativeTorque(0, 0, -FlyRight/5f); }//rotate drone right

            { rBody.AddRelativeForce(0, 0, Backward / -1); rBody.AddRelativeTorque(-Backward/5f, 0, 0); }// drone fly backward

            { rBody.AddRelativeForce(0, Up, 0); }//drone fly up

            { rBody.AddRelativeForce(0, Down / -1, 0); }//drone fly down
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if it hits the city
       if(collision.collider.CompareTag("Environment"))
        {
            //Penalize
            AddReward(-1f);
            EndEpisode();
        }
       //If it reaches the target
       if(collision.collider.CompareTag("Target"))
        {
            //Reward
            AddReward(2f);
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        if (Input.GetKey(KeyCode.W)) //drone fly forward
        {
            actionsOut[0] = 5f;
        }
        else { actionsOut[0] = 0f; }
        if (Input.GetKey(KeyCode.LeftArrow)) //rotate drone left
        {
            actionsOut[1] = 5f;
        }
        else { actionsOut[1] = 0f; }
        if (Input.GetKey(KeyCode.RightArrow)) //rotate drone right
        {
            actionsOut[2] = 5f;
        }
        else { actionsOut[2] = 0f; }
        if (Input.GetKey(KeyCode.S)) // drone fly backward
        {
            actionsOut[3] = 5f;
        }
        else { actionsOut[3] = 0f; }
        if (Input.GetKey(KeyCode.UpArrow)) //drone fly up
        {
            actionsOut[4] = 5f;
        }
        else { actionsOut[4] = 0f; }
        if (Input.GetKey(KeyCode.DownArrow)) //drone fly down
        {
            actionsOut[5] = 5f;
        }
        else { actionsOut[5] = 0f; }
        if (Input.GetKey(KeyCode.D)) //tilt drone right
        {
            actionsOut[6] = 5f;
        }
        else { actionsOut[6] = 0f; }
        if (Input.GetKey(KeyCode.A)) //tilt drone left
        {
            actionsOut[7] = 5f;
        }
        else { actionsOut[7] = 0f; }
    }



}