using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic;

namespace CapaceteVR.Input
{
    /// <summary>
    /// Abstrai a leitura dos controles XR, isolando o resto do sistema
    /// de detalhes de hardware específico.
    /// </summary>
    public class VRControllerInput : MonoBehaviour, IVRInput
    {
        [SerializeField, Range(0f, 0.5f)]
        private float deadzone = 0.15f;

        private InputDevice _leftController;
        private InputDevice _rightController;

        private void Start() => RefreshDevices();

        private void Update()
        {
            if (!_leftController.isValid || !_rightController.isValid)
                RefreshDevices();
        }

        public Vector2 LeftThumbstick  => ReadAxis(_leftController);
        public Vector2 RightThumbstick => ReadAxis(_rightController);

        private void RefreshDevices()
        {
            _leftController  = FindDevice(XRNode.LeftHand);
            _rightController = FindDevice(XRNode.RightHand);
        }

        private static InputDevice FindDevice(XRNode node)
        {
            var list = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(node, list);
            return list.Count > 0 ? list[0] : default;
        }

        private Vector2 ReadAxis(InputDevice device)
        {
            if (!device.isValid) return Vector2.zero;
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out var axis);
            return axis.magnitude < deadzone ? Vector2.zero : axis;
        }
    }
}
