using System;
using UnityEngine;

namespace SF
{
	[Serializable]
	public struct CollisionInfo
	{
		public RaycastHit2D GroundedHit;
		public RaycastHit2D CeilingHit;
		public RaycastHit2D RightHit;
		public RaycastHit2D LeftHit;

		public bool IsCollidingRight;
		public bool IsCollidingLeft;
		public bool IsCollidingAbove;
		public bool IsCollidingBelow;

        [SerializeField] private RaycastHit2D _climbableSurfaceHit;
        public RaycastHit2D ClimbableSurfaceHit
        {
            get { return _climbableSurfaceHit; }
            set
            {
                if(value)
                {
					if(value.collider.TryGetComponent(out ClimbableSurface climable))
					{
                        _climbableSurfaceHit = value;
                        ClimbableSurface = climable;
					}
					else
					{
						ClimbableSurface = null;
					}
                }
				else
					ClimbableSurface = null;
            }
        }

        [SerializeField] private ClimbableSurface _climableSurface;
        public ClimbableSurface ClimbableSurface
		{
			get { return _climableSurface; }
			set 
			{ 
				if(value == null)
					WasClimbing = false;
				_climableSurface = value;
			}
		}


		// The below is for seeing if we were colliding in a direction on the previous frame.
		[NonSerialized] public bool WasCollidingRight;
		[NonSerialized] public bool WasCollidingLeft;
		[NonSerialized] public bool WasCollidingAbove;
		[NonSerialized] public bool WasCollidingBelow;
		[NonSerialized] public bool WasClimbing;

		// These are for invoking actions on the frame a new collision takes place.
		public Action OnCollidedRight;
		public Action OnCollidedLeft;
		public Action OnCollidedAbove;
		public Action OnCollidedBelow;

		private RaycastHit2D _emptyHit2D;
	}
}
