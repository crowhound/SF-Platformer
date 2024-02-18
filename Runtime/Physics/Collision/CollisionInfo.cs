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

		// The below is for seeing if we were colliding in a direction on the previous frame.
		public bool WasCollidingRight;
		public bool WasCollidingLeft;
		public bool WasCollidingAbove;
		public bool WasCollidingBelow;

		// These are for invoking actions on the frame a new collision takes place.
		public Action OnCollidedRight;
		public Action OnCollidedLeft;
		public Action OnCollidedAbove;
		public Action OnCollidedBelow;
	}
}
