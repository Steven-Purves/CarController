using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    Vector2 input;
    bool onhandBrake;
    float steeringAngle;
    
    WheelCollider[] wheelColliders;
    Transform[] wheelsTransforms;

    Rigidbody rb;

    [SerializeField]
    float maxSteerAngle = 30;
    [SerializeField]
    float motorForce = 1200;
    [SerializeField]
    float handBrake = 50;
    [SerializeField]
    float frontBrake = 50;

    public bool fourWheeldrive = true;
    
    public void GetInput()
    {
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        onhandBrake = Input.GetKey(KeyCode.Space);
    }
    void Steer()
    {
        steeringAngle = maxSteerAngle * input.x;

        for (int i = 0; i < 2; i++)
        {
            wheelColliders[i].steerAngle = steeringAngle;
        } 
    }
    void Accelerate()
    {
        int driveNum = fourWheeldrive ? 4 : 2;

        var velocity = rb.velocity;
        var localVel = transform.InverseTransformDirection(velocity);

        for (int i = 0; i < driveNum; i++)
        {
            wheelColliders[i].brakeTorque = 0;

            if (localVel.z > 0)
            {
                if (input.y >= 0)
                {
                    wheelColliders[i].motorTorque = input.y * motorForce;
                }
                else
                {
                   // wheelColliders[i].motorTorque = 0;
                    if(i < 2)
                    {
                        wheelColliders[i].brakeTorque = frontBrake;
                    }
                }
            }
            else
            {
                wheelColliders[i].motorTorque = i < 2 ? input.y * motorForce : 0;
            }
        }
    }
    void HandBrake()
    {
        for (int i = 2; i < wheelColliders.Length; i++)
        {
            wheelColliders[i].motorTorque = onhandBrake ? 0 : wheelColliders[i].motorTorque;
            wheelColliders[i].brakeTorque = onhandBrake ? handBrake : 0;
        }

    }
    void UpdateWheelPositions()
    {
        for (int i = 0; i < wheelColliders.Length; i++)
        {
            UpdateWheelPosition(wheelColliders[i], wheelsTransforms[i + 1]);
        }
    }
    void UpdateWheelPosition(WheelCollider _collider, Transform _transform)
    {
        Vector3 pos = _transform.position;
        Quaternion quat = _transform.rotation;

        _collider.GetWorldPose(out pos, out quat);

        _transform.position = pos;
        _transform.rotation = quat;

    }
    private void FixedUpdate()
    {
        GetInput();
        Steer();
        Accelerate();
        UpdateWheelPositions();
        HandBrake();
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        GameObject wheels = GameObject.Find("WheelsGeo");
        wheelsTransforms = wheels.GetComponentsInChildren<Transform>();

        wheelColliders = GetComponentsInChildren<WheelCollider>();
    }
}
