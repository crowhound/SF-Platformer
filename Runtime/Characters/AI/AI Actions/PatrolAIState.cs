using SF.Characters.Controllers;
using SF.Platformer.Utilities;

using UnityEngine;

namespace SF
{
    public class PatrolAIState : MonoBehaviour
	{
        private Controller2D _controller;
        public bool StartingRight = true;
		public bool DoesTurnOnHoles = true;
		private bool _isHoleAhead;

		private void Awake()
		{
			_controller = GetComponent<Controller2D>();
		}

		private void Start()
		{
			if(_controller == null)
				return;

			_controller.CollisionInfo.OnCollidedLeft += OnCollidingLeft;
			_controller.CollisionInfo.OnCollidedRight += OnCollidingRight;

			_controller.Direction = StartingRight
				? Vector2.right : Vector2.left;
		}
        private void FixedUpdate()
        {
            if(_controller == null || !DoesTurnOnHoles)
				return;

			if(_controller.Direction == Vector2.left)
				_isHoleAhead = !Physics2D.Raycast(_controller.Bounds.BottomLeft(), Vector2.down,_controller.CollisionController.VerticalRayDistance, _controller.PlatformFilter.layerMask);
			else
                _isHoleAhead = !Physics2D.Raycast(_controller.Bounds.BottomRight(), Vector2.down, _controller.CollisionController.VerticalRayDistance, _controller.PlatformFilter.layerMask);

            if(_isHoleAhead && _controller.CharacterState.MovementState != Characters.MovementState.Falling)
			{
				_controller.ChangeDirection();
			}
        }
        private void OnCollidingLeft()
		{
			if(_controller.Direction == Vector2.left)
				_controller.ChangeDirection();
		}

		private void OnCollidingRight()
		{
			if(_controller.Direction == Vector2.right)
				_controller.ChangeDirection();
		}
	}
}