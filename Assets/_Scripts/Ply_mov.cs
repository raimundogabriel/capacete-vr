using UnityEngine.XR;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Ply_mov : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotationSpeed = 90f; // graus por segundo
    public Transform head;

    private InputDevice leftController;
    private InputDevice rightController;

    void Start()
    {
        GetDevices();
    }

    void GetDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0)
            leftController = devices[0];

        devices.Clear();

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0)
            rightController = devices[0];
    }

    void Update()
    {
        // reconecta se perder tracking
        if (!leftController.isValid || !rightController.isValid)
            GetDevices();

        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector2 inputAxis;
        if (leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
        {
            Vector3 forward = head.forward;
            Vector3 right = head.right;

            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            Vector3 direction = forward * inputAxis.y + right * inputAxis.x;

            transform.position += direction * speed * Time.deltaTime;
        }
    }

    void HandleRotation()
    {
        Vector2 inputAxis;
        if (rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
        {
            float turn = inputAxis.x; // eixo horizontal do analógico direito

            // rotação suave
            transform.Rotate(0, turn * rotationSpeed * Time.deltaTime, 0);
        }
    }
}