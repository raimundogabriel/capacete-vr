using CapaceteVR.Input;
using CapaceteVR.Locomotion;
using NUnit.Framework;
using UnityEngine;

namespace CapaceteVR.Tests.Locomotion
{
    /// <summary>
    /// Testes EditMode para SnapTurn usando fake de input.
    /// </summary>
    public class SnapTurnTests
    {
        private GameObject _playerObject;
        private SnapTurn   _snapTurn;
        private FakeVRInput _fakeInput;

        [SetUp]
        public void SetUp()
        {
            _playerObject = new GameObject("Player");
            _snapTurn     = _playerObject.AddComponent<SnapTurn>();
            _fakeInput    = new FakeVRInput();
            _snapTurn.InjectDependencies(_fakeInput);
        }

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(_playerObject);

        [Test]
        public void WhenRightThumbstickPushedRight_RotatesPositive()
        {
            var before = _playerObject.transform.eulerAngles.y;
            _fakeInput.RightThumbstick = new Vector2(1f, 0f);

            _snapTurn.SendMessage("TrySnap", SendMessageOptions.DontRequireReceiver);
            // Chama via reflexão pois TrySnap é private; usamos Update no PlayMode
            InvokePrivateUpdate(_snapTurn);

            var after = _playerObject.transform.eulerAngles.y;
            Assert.That(after, Is.GreaterThan(before).Or.EqualTo(before + 30f).Within(0.1f));
        }

        [Test]
        public void WhenRightThumbstickPushedLeft_RotatesNegative()
        {
            _fakeInput.RightThumbstick = new Vector2(-1f, 0f);
            InvokePrivateUpdate(_snapTurn);

            var y = _playerObject.transform.eulerAngles.y;
            // -30 em eulerAngles vira 330
            Assert.That(y, Is.EqualTo(330f).Within(0.1f));
        }

        [Test]
        public void SecondCallWithSameDirection_DoesNotSnapAgain()
        {
            _fakeInput.RightThumbstick = new Vector2(1f, 0f);
            InvokePrivateUpdate(_snapTurn);
            var afterFirst = _playerObject.transform.eulerAngles.y;

            // Segundo frame, botão ainda pressionado
            InvokePrivateUpdate(_snapTurn);
            var afterSecond = _playerObject.transform.eulerAngles.y;

            Assert.That(afterSecond, Is.EqualTo(afterFirst).Within(0.001f),
                "Snap não deve repetir enquanto o botão estiver pressionado.");
        }

        [Test]
        public void WhenThumbstickBelowThreshold_DoesNotSnap()
        {
            _fakeInput.RightThumbstick = new Vector2(0.5f, 0f); // abaixo de 0.7
            InvokePrivateUpdate(_snapTurn);

            Assert.That(_playerObject.transform.eulerAngles.y, Is.EqualTo(0f).Within(0.001f));
        }

        private static void InvokePrivateUpdate(MonoBehaviour target)
        {
            var method = target.GetType().GetMethod("Update",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method?.Invoke(target, null);
        }
    }

    /// <summary>Implementação de IVRInput para uso em testes.</summary>
    internal class FakeVRInput : IVRInput
    {
        public Vector2 LeftThumbstick  { get; set; }
        public Vector2 RightThumbstick { get; set; }
    }
}
