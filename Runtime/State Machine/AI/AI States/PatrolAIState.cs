using SF.Characters.Controllers;
using SF.Platformer.Utilities;
using SF.StateMachine.Core;
using SF.Weapons;

using UnityEngine;

namespace SF.StateMachine
{
    public class PatrolAIState : StateCore
    {
        private Controller2D _controller;
        public bool StartingRight = true;
		public bool DoesTurnOnHoles = true;
		
		[SerializeField] private bool _isHoleAhead;

        protected override void OnInit(Controller2D controller2D)
		{
			_controller = controller2D;
		}


        protected override void OnStart()
		{
			if(_controller == null)
				return;

			_controller.CollisionInfo.OnCollidedLeft += OnCollidingLeft;
			_controller.CollisionInfo.OnCollidedRight += OnCollidingRight;

			_controller.Direction = StartingRight
				? Vector2.right : Vector2.left;
		}

        protected override void OnUpdateState()
        {
			HoleDetection();
        }

		private void HoleDetection()
		{
			if(_controller == null || !DoesTurnOnHoles)
				return;

			RaycastHit2D hit2D = new RaycastHit2D();
			if(_controller.Direction == Vector2.left)
			{
                hit2D = Physics2D.Raycast(_controller.Bounds.BottomLeft(),
						Vector2.down,
						_controller.CollisionController.VerticalRayDistance + _controller.CollisionController.SkinWidth,
						_controller.PlatformFilter.layerMask
					);
			}
			else if(_controller.Direction == Vector2.right)
			{

                hit2D = Physics2D.Raycast(_controller.Bounds.BottomRight(),
						Vector2.down,
						_controller.CollisionController.VerticalRayDistance + _controller.CollisionController.SkinWidth,
						_controller.PlatformFilter.layerMask
					);
			}

			_isHoleAhead = !hit2D;

            if(_isHoleAhead)
            {
                _controller.ChangeDirection();
            }
        }
        
		private void OnCollidingLeft()
		{
			if(_controller.Direction == Vector2.left)
				_controller.SetDirection(1);
		}
		private void OnCollidingRight()
		{
            if(_controller.Direction == Vector2.right)
                _controller.SetDirection(-1);
        }
	}
}