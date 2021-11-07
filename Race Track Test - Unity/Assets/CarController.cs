using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

public class CarController : MonoBehaviour
{
    private PlayerControls controls;

    public float horizontalInput { get; private set; }
    public float verticalInput { get; private set; }
    private float steerAngle;
    private bool isBreaking;

    public WheelCollider frontLeftWheelCollider;
    public WheelCollider frontRightWheelCollider;
    public WheelCollider rearLeftWheelCollider;
    public WheelCollider rearRightWheelCollider;
    public Transform frontLeftWheelTransform;
    public Transform frontRightWheelTransform;
    public Transform rearLeftWheelTransform;
    public Transform rearRightWheelTransform;

    public float maxSteeringAngle = 30f;
    public float motorForce = 50f;
    public float brakeForce = 0f;

    public void Awake () {
        controls = new PlayerControls();
        // controls.Car.Move.performed += ctx => Move(ctx.ReadValue<Vector2>());
        // controls.Car.Move.canceled += ctx => Move(ctx.ReadValue<Vector2>());
        // controls.Car.Break.performed += ctx => Break(ctx.ReadValue<float>());
        // controls.Car.Break.canceled += ctx => Break(ctx.ReadValue<float>());
    }

    void OnEnable() {
        controls.Enable();
    }

    void OnDisable() {
        controls.Disable();
    }

    private void FixedUpdate()
    {
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }

    void Update () {
        Debug.Log("Horizontal: " + horizontalInput + " Vertical: " + verticalInput);
    }

    public String PrintByteArray(byte[] bytes)
    {
        var sb = "new byte[] { ";
        foreach (var b in bytes)
        {
            sb += b.ToString() + ", ";
        }
        sb += "}";
        return sb;
    }

    public void GetInputY(InputAction.CallbackContext context)
    {
        verticalInput = context.ReadValue<float>();
    }

    public void GetInputX(InputAction.CallbackContext context)
    {
        horizontalInput = context.ReadValue<float>();
    }

    // takes a vector and inverse of each component and normalizes it
    Vector2 inverseVector(Vector2 v)
    {
        Func<float, float> k = x => {
            if (x < 0)
                return - (1 + x);
            else if (x > 0)
                return + (1 - x);
            else
                return Mathf.Sign(x);
        };
        return new Vector2(k(v.x), k(v.y));
    }

    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = steerAngle;
        frontRightWheelCollider.steerAngle = steerAngle;
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

        brakeForce = isBreaking ? 3000f : 0f;
        frontLeftWheelCollider.brakeTorque = brakeForce;
        frontRightWheelCollider.brakeTorque = brakeForce;
        rearLeftWheelCollider.brakeTorque = brakeForce;
        rearRightWheelCollider.brakeTorque = brakeForce;
    }

    private void UpdateWheels()
    {
        UpdateWheelPos(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateWheelPos(frontRightWheelCollider, frontRightWheelTransform);
        UpdateWheelPos(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateWheelPos(rearRightWheelCollider, rearRightWheelTransform);
    }

    private void UpdateWheelPos(WheelCollider wheelCollider, Transform trans)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        trans.rotation = rot;
        trans.position = pos;
    }

}