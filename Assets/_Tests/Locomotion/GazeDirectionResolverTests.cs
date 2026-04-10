using CapaceteVR.Locomotion;
using NUnit.Framework;
using UnityEngine;

namespace CapaceteVR.Tests.Locomotion
{
    /// <summary>
    /// Testes EditMode para GazeDirectionResolver.
    /// Não requerem contexto de cena ou hardware XR.
    /// </summary>
    public class GazeDirectionResolverTests
    {
        private GameObject _headObject;
        private Transform  _head;

        [SetUp]
        public void SetUp()
        {
            _headObject = new GameObject("Head");
            _head = _headObject.transform;
        }

        [TearDown]
        public void TearDown() => Object.DestroyImmediate(_headObject);

        [Test]
        public void WhenLookingNorth_AndPushingForward_MovesNorth()
        {
            _head.rotation = Quaternion.LookRotation(Vector3.forward);
            var result = GazeDirectionResolver.Resolve(_head, Vector2.up);

            Assert.That(result.x, Is.EqualTo(0f).Within(0.001f));
            Assert.That(result.z, Is.EqualTo(1f).Within(0.001f));
            Assert.That(result.y, Is.EqualTo(0f).Within(0.001f), "Componente Y deve ser zero.");
        }

        [Test]
        public void WhenLookingEast_AndPushingForward_MovesEast()
        {
            _head.rotation = Quaternion.LookRotation(Vector3.right);
            var result = GazeDirectionResolver.Resolve(_head, Vector2.up);

            Assert.That(result.x, Is.EqualTo(1f).Within(0.001f));
            Assert.That(result.z, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void WhenLookingNorth_AndPushingRight_MovesEast()
        {
            _head.rotation = Quaternion.LookRotation(Vector3.forward);
            var result = GazeDirectionResolver.Resolve(_head, Vector2.right);

            Assert.That(result.x, Is.EqualTo(1f).Within(0.001f));
            Assert.That(result.z, Is.EqualTo(0f).Within(0.001f));
        }

        [Test]
        public void WhenLookingDown_MovementIsStillHorizontal()
        {
            _head.rotation = Quaternion.LookRotation(Vector3.down);
            var result = GazeDirectionResolver.Resolve(_head, Vector2.up);

            Assert.That(result.y, Is.EqualTo(0f).Within(0.001f), "Olhar para baixo não deve gerar movimento vertical.");
            Assert.That(result.magnitude, Is.GreaterThan(0f), "Deve ainda produzir movimento horizontal.");
        }

        [Test]
        public void WhenThumbstickIsZero_ReturnsZeroVector()
        {
            _head.rotation = Quaternion.LookRotation(Vector3.forward);
            var result = GazeDirectionResolver.Resolve(_head, Vector2.zero);

            Assert.That(result, Is.EqualTo(Vector3.zero));
        }

        [Test]
        public void DiagonalInput_ProducesDiagonalMovement()
        {
            _head.rotation = Quaternion.LookRotation(Vector3.forward);
            var diagonal = new Vector2(1f, 1f).normalized;
            var result = GazeDirectionResolver.Resolve(_head, diagonal);

            Assert.That(result.x, Is.GreaterThan(0f));
            Assert.That(result.z, Is.GreaterThan(0f));
            Assert.That(result.y, Is.EqualTo(0f).Within(0.001f));
        }
    }
}
