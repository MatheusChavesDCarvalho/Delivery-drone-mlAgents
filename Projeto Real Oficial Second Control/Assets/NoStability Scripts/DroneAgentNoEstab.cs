using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System.Net;
using System;

public class DroneAgentNoEstab : Agent
{
    Rigidbody rBody;
    //Rigidbody Prop1;
    //Rigidbody Prop2;
    //Rigidbody Prop3;
    //Rigidbody Prop4;

    float diam = 10f; //propeller used --> 10''x5''
    float pitch = 5f;

    float LFP_vel = 0;
    float RFP_vel = 0;
    float LBP_vel = 0;
    float RBP_vel = 0;

    double LFP_for = 0;
    double RFP_for = 0;
    double LBP_for = 0;
    double RBP_for = 0;

    //float Forward = 0;
    //float FlyLeft = 0;
    //float FlyRight = 0;
    //float Backward = 0;
    //float Up = 0;
    //float Down = 0;
    //float TiltRight = 0;
    //float TiltLeft = 0;
    float distanceToTargetBefore;


    public GameObject Prop1_LFP; //propeller 1 - Left Front Propeler
    public GameObject Prop2_RFP; //propeller 2
    public GameObject Prop3_LBP; //propeller 3
    public GameObject Prop4_RBP; //propeller 4

    private float AngAccelX=0;
    private float AngAccelZ=0;
    private float AngVelBeforeX=0;
    private float AngVelBeforeZ=0;
    private float AngAccelXBefore=0;
    private float AngAccelZBefore=0;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
        //Prop1 = Prop1_LFP.GetComponent<Rigidbody>();
        //Prop2 = Prop2_RFP.GetComponent<Rigidbody>();
        //Prop3 = Prop3_LBP.GetComponent<Rigidbody>();
        //Prop4 = Prop4_RBP.GetComponent<Rigidbody>();
    }



    public Transform Target;
    public override void OnEpisodeBegin()
    {


        
        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(0, 5f, 0);
        this.transform.localRotation = new Quaternion(0, 0, 0, 1);


        // Move the target to a new spot
        int pos = UnityEngine.Random.Range(0, 16);
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
                Target.localPosition = new Vector3(-62.1f, 80.3f, -233.7f);
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

        // Agent Rotation
        sensor.AddObservation(transform.rotation);
    }

    public float SpeedMod = 10000;
    

    public override void OnActionReceived(float[] vectorAction)
    {
        AddReward(-0.01f);

        // Actions, size = 8

        //Forward = vectorAction[0] * ForceMod;
        //FlyLeft = vectorAction[1] * ForceMod;
        //FlyRight = vectorAction[2] * ForceMod;
        //Backward = vectorAction[3] * ForceMod;
        //Up = vectorAction[4] * ForceMod;
        //Down = vectorAction[5] * ForceMod;
        //TiltRight = vectorAction[6] * ForceMod;
        //TiltLeft = vectorAction[7] * ForceMod;


        // Rewards
        float distanceToTargetNow = Vector3.Distance(this.transform.localPosition, Target.localPosition);

        // Reached target
        if (distanceToTargetNow < distanceToTargetBefore)
        {
            AddReward(0.005f);
        }
        else
        {
            AddReward(-0.01f);
        }

        distanceToTargetBefore = distanceToTargetNow;

        //Rotation angles
        if (-30 < transform.rotation.x && transform.rotation.x < 30)
        {
            AddReward(0.005f);
        }
        else
        {
            AddReward(-0.003f);
        }

        if (-30 < transform.rotation.z && transform.rotation.z < 30)
        {
            AddReward(0.005f);
        }
        else
        {
            AddReward(-0.003f);
        }

        //stability

        
        AngAccelX = Math.Abs(rBody.angularVelocity.x - AngVelBeforeX)/Time.deltaTime;
        AngAccelZ = Math.Abs(rBody.angularVelocity.z - AngVelBeforeZ)/Time.deltaTime;
        if (AngAccelX < AngAccelXBefore)
        {
            AddReward(0.005f);
        }
        else
        {
            AddReward(-0.003f);
        }

        if (AngAccelZ < AngAccelZBefore)
        {
            AddReward(0.005f);
        }
        else
        {
            AddReward(-0.003f);
        }
        AngVelBeforeX = rBody.angularVelocity.x;
        AngVelBeforeZ = rBody.angularVelocity.z;
        AngAccelXBefore = AngAccelX;
        AngAccelZBefore = AngAccelZ;



        //Drone Controls (with stability control)

        //{ rBody.AddRelativeTorque(0, TiltLeft / -1, 0); }//tilt drone left

        //{ rBody.AddRelativeTorque(0, TiltRight, 0); }//tilt drone right

        //{ rBody.AddRelativeForce(0, 0, Forward); rBody.AddRelativeTorque(Forward / 5f, 0, 0); }//drone fly forward

        //{ rBody.AddRelativeForce(FlyLeft / -1, 0, 0); rBody.AddRelativeTorque(0, 0, FlyLeft / 5f); }//rotate drone left

        //{ rBody.AddRelativeForce(FlyRight, 0, 0); rBody.AddRelativeTorque(0, 0, -FlyRight / 5f); }//rotate drone right

        //{ rBody.AddRelativeForce(0, 0, Backward / -1); rBody.AddRelativeTorque(-Backward / 5f, 0, 0); }// drone fly backward

        //{ rBody.AddRelativeForce(0, Up, 0); }//drone fly up

        //{ rBody.AddRelativeForce(0, Down / -1, 0); }//drone fly down



        //Drone Controls (without stability control)
        //speed in RPM, high SpeedMod is to maintain small values in the neural network
        LFP_vel = vectorAction[0] * SpeedMod; //Prop1
        RFP_vel = vectorAction[1] * SpeedMod; //Prop2
        LBP_vel = vectorAction[2] * SpeedMod; //Prop3
        RBP_vel = vectorAction[3] * SpeedMod; //Prop4

        LFP_for = (1.255f * (Mathf.PI * Math.Pow((0.0254f * diam), 2)) / 4f) * Math.Pow((LFP_vel * 0.0254f * pitch / 60f), 2);
        RFP_for = (1.255f * (Mathf.PI * Math.Pow((0.0254f * diam), 2)) / 4f) * Math.Pow((RFP_vel * 0.0254f * pitch / 60f), 2);
        LBP_for = (1.255f * (Mathf.PI * Math.Pow((0.0254f * diam), 2)) / 4f) * Math.Pow((LBP_vel * 0.0254f * pitch / 60f), 2);
        RBP_for = (1.255f * (Mathf.PI * Math.Pow((0.0254f * diam), 2)) / 4f) * Math.Pow((RBP_vel * 0.0254f * pitch / 60f), 2);



        rBody.AddForceAtPosition(transform.up * (float)LFP_for, Prop1_LFP.transform.position);
        rBody.AddForceAtPosition(transform.up * (float)RFP_for, Prop2_RFP.transform.position);
        rBody.AddForceAtPosition(transform.up * (float)LBP_for, Prop3_LBP.transform.position);
        rBody.AddForceAtPosition(transform.up * (float)RBP_for, Prop4_RBP.transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        //if it hits the city
        if (collision.collider.CompareTag("Environment"))
        {
            //Penalize
            AddReward(-1f);
            EndEpisode();
        }
        //If it reaches the target
        if (collision.collider.CompareTag("Target"))
        {
            //Reward
            AddReward(3f);
            EndEpisode();
        }
    }

    public override void Heuristic(float[] actionsOut)
    {
        //if (Input.GetKey(KeyCode.W)) //drone fly forward
        //{
        //    actionsOut[0] = 5f;
        //}
        //else { actionsOut[0] = 0f; }
        //if (Input.GetKey(KeyCode.LeftArrow)) //rotate drone left
        //{
        //    actionsOut[1] = 5f;
        //}
        //else { actionsOut[1] = 0f; }
        //if (Input.GetKey(KeyCode.RightArrow)) //rotate drone right
        //{
        //    actionsOut[2] = 5f;
        //}
        //else { actionsOut[2] = 0f; }
        //if (Input.GetKey(KeyCode.S)) // drone fly backward
        //{
        //    actionsOut[3] = 5f;
        //}
        //else { actionsOut[3] = 0f; }
        //if (Input.GetKey(KeyCode.UpArrow)) //drone fly up
        //{
        //    actionsOut[4] = 5f;
        //}
        //else { actionsOut[4] = 0f; }
        //if (Input.GetKey(KeyCode.DownArrow)) //drone fly down
        //{
        //    actionsOut[5] = 5f;
        //}
        //else { actionsOut[5] = 0f; }
        //if (Input.GetKey(KeyCode.D)) //tilt drone right
        //{
        //    actionsOut[6] = 5f;
        //}
        //else { actionsOut[6] = 0f; }
        //if (Input.GetKey(KeyCode.A)) //tilt drone left
        //{
        //    actionsOut[7] = 5f;
        //}
        //else { actionsOut[7] = 0f; }




        if (Input.GetKey(KeyCode.W)) //Prop1 - Left Front
        {
            actionsOut[0] = 1f;
        }
        else { actionsOut[0] = 0f; }
        if (Input.GetKey(KeyCode.R)) //Prop2 - Right Front
        {
            actionsOut[1] = 1f;
        }
        else { actionsOut[1] = 0f; }
        if (Input.GetKey(KeyCode.X)) //Prop3 - Left Back
        {
            actionsOut[2] = 1f;
        }
        else { actionsOut[2] = 0f; }
        if (Input.GetKey(KeyCode.V)) //Prop4 - Right Back
        {
            actionsOut[3] = 1f;
        }
        else { actionsOut[3] = 0f; }
    }



}