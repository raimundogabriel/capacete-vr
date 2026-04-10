using CapaceteVR.Input;
using UnityEngine;

namespace CapaceteVR.Locomotion
{
    /// <summary>
    /// Rotaciona o personagem em incrementos discretos (snap turn)
    /// a partir do analógico direito, reduzindo enjoo em VR.
    /// </summary>
    public class SnapTurn : MonoBehaviour
    {
        [SerializeField] private float snapAngle = 30f;
        [SerializeField, Range(0f, 1f)] private float triggerThreshold = 0.7f;

        private IVRInput _input;
        private bool _waitingForRelease;

        private void Awake() => _input = GetComponent<IVRInput>();

        private void Update() => TrySnap();

        private void TrySnap()
        {
            var x = _input.RightThumbstick.x;

            if (Mathf.Abs(x) < triggerThreshold)
            {
                _waitingForRelease = false;
                return;
            }

            if (_waitingForRelease) return;

            var direction = x > 0f ? 1f : -1f;
            transform.Rotate(Vector3.up, direction * snapAngle, Space.World);
            _waitingForRelease = true;
        }

#if UNITY_EDITOR
        public void InjectDependencies(IVRInput input) => _input = input;
#endif
    }
}
