using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

[RequireComponent(typeof(CharacterController))]
public class Ply_mov : MonoBehaviour
{
    public float speed = 2.0f;
    public float rotationSpeed = 90f;
    public Transform head;

    [Range(0f, 0.5f)]
    public float deadzone = 0.15f;

    private CharacterController _cc;
    private InputDevice leftController;
    private InputDevice rightController;
    private float _verticalVelocity = 0f;

    void Start()
    {
        _cc = GetComponent<CharacterController>();
        GetDevices();
    }

    void GetDevices()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, devices);
        if (devices.Count > 0) leftController = devices[0];

        devices.Clear();

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, devices);
        if (devices.Count > 0) rightController = devices[0];
    }

    void Update()
    {
        if (!leftController.isValid || !rightController.isValid)
            GetDevices();

        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        Vector2 inputAxis;
        if (!leftController.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
            return;

        // Deadzone — ignora drift pequeno do analógico
        if (inputAxis.magnitude < deadzone)
            inputAxis = Vector2.zero;

        // Direção baseada em onde a cabeça está olhando
        Vector3 forward = head.forward;
        Vector3 right   = head.right;

        forward.y = 0f;
        right.y   = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * inputAxis.y) + (right * inputAxis.x);

        // Gravidade
        if (_cc.isGrounded)
            _verticalVelocity = -0.5f;
        else
            _verticalVelocity += Physics.gravity.y * Time.deltaTime;

        moveDir.y = _verticalVelocity;

        // Move com o CharacterController (respeita colisão e física)
        _cc.Move(moveDir * speed * Time.deltaTime);
    }

    void HandleRotation()
    {
        Vector2 inputAxis;
        if (!rightController.TryGetFeatureValue(CommonUsages.primary2DAxis, out inputAxis))
            return;

        // Deadzone no eixo horizontal
        if (Mathf.Abs(inputAxis.x) < deadzone)
            return;

        transform.Rotate(0f, inputAxis.x * rotationSpeed * Time.deltaTime, 0f, Space.World);
    }
}