using UnityEngine;
using Prime31;
using Zenject;

namespace GG
{
    public class CharacterMotor : ITickable
    {
        private CharacterController2D _controller;
        private Settings _settings;

        private Vector2 _velocity = new Vector2();

        public Vector2 Velocity { get { return _velocity; } }

        public CharacterController2D.CharacterCollisionState2D CollisionState { get { return _controller.collisionState; } }
        public bool IsGrounded { get { return _controller.isGrounded; } }

        public CharacterMotor(CharacterController2D controller, Settings settings)
        {
            _controller = controller;
            _settings = settings;
        }

        public void Tick()
        {
            applyGravity();
            _controller.move(_velocity * Time.deltaTime);
        }

        public void SetVelocityX(float x)
        {
            _velocity.x = x;
        }

        public void SetVelocityY(float y)
        {
            _velocity.y = y;
        }

        private void applyGravity()
        {
            if (!_controller.isGrounded)
                _velocity.y -= _settings.gravity * Time.deltaTime;

            _velocity.y = Mathf.Max(_velocity.y, -_settings.maxFallSpeed);
        }

        [System.Serializable]
        public class Settings
        {
            public float gravity;
            public float maxFallSpeed;
        }
    }
}