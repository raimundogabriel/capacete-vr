using CapaceteVR.Input;
using UnityEngine;

namespace CapaceteVR.Locomotion
{
    /// <summary>
    /// Move o personagem na direção em que o HMD está olhando,
    /// usando o analógico esquerdo como intensidade/direção.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class GazeLocomotion : MonoBehaviour
    {
        [SerializeField] private Transform headTransform;
        [SerializeField] private float speed = 2f;

        private CharacterController _controller;
        private IVRInput _input;

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();
            _input      = GetComponent<IVRInput>();
        }

        private void Update() => Move();

        private void Move()
        {
            var moveDirection = GazeDirectionResolver.Resolve(headTransform, _input.LeftThumbstick);
            _controller.Move(moveDirection * speed * Time.deltaTime);
        }

#if UNITY_EDITOR
        /// <summary>Injeção de dependência usada nos testes.</summary>
        public void InjectDependencies(IVRInput input, CharacterController controller, Transform head)
        {
            _input       = input;
            _controller  = controller;
            headTransform = head;
        }
#endif
    }
}
