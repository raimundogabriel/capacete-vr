using UnityEngine;

namespace CapaceteVR.Locomotion
{
    /// <summary>
    /// Calcula os vetores de movimento a partir da orientação do HMD (cabeça do usuário).
    /// Ignora o componente vertical do olhar para que o movimento seja sempre horizontal.
    /// </summary>
    public static class GazeDirectionResolver
    {
        /// <summary>
        /// Retorna o vetor de movimento no plano XZ baseado no olhar do HMD
        /// e no input do analógico.
        /// </summary>
        /// <param name="headTransform">Transform da câmera/HMD.</param>
        /// <param name="thumbstick">Input normalizado do analógico (x = lateral, y = frontal).</param>
        /// <returns>Vetor de deslocamento no plano horizontal, sem normalização de magnitude.</returns>
        public static Vector3 Resolve(Transform headTransform, Vector2 thumbstick)
        {
            var forward = ProjectOntoHorizontalPlane(headTransform.forward);
            var right   = ProjectOntoHorizontalPlane(headTransform.right);

            return (forward * thumbstick.y) + (right * thumbstick.x);
        }

        private static Vector3 ProjectOntoHorizontalPlane(Vector3 direction)
        {
            direction.y = 0f;
            return direction.normalized;
        }
    }
}
