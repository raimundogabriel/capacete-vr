using UnityEngine;

namespace CapaceteVR.Input
{
    /// <summary>
    /// Contrato de leitura de input VR — permite fakes em testes
    /// sem depender de hardware físico.
    /// </summary>
    public interface IVRInput
    {
        /// <summary>Analógico esquerdo, já com deadzone aplicado.</summary>
        Vector2 LeftThumbstick { get; }

        /// <summary>Analógico direito, já com deadzone aplicado.</summary>
        Vector2 RightThumbstick { get; }
    }
}
