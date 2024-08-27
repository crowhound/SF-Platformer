using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

#if SF_Utilities
using SF.Utilities;
#else
using SF.Platformer.Utilities;

using Unity.Profiling;

using UnityEditor;


#endif
using UnityEngine;

namespace SF.Characters.Controllers
{
	public class PlayerController : GroundedController2D
    {

#if UNITY_EDITOR

		static readonly ProfilerMarker s_OnDrawGizmosMarker = new ProfilerMarker(ProfilerCategory.Render,"SF.Gizmos.Draw");
		private readonly List<Vector3> _listOfPoints = new();

        public void OnDrawGizmos()
		{
			s_OnDrawGizmosMarker.Begin();
            _listOfPoints.Clear();
            _boxCollider = (_boxCollider == null) ? GetComponent<BoxCollider2D>() : _boxCollider;
			Bounds = _boxCollider.bounds;

			Vector2 startPosition;
			float stepPercent;
			int numberOfRays = CollisionController.VerticalRayAmount;
			Vector2 origin = Bounds.BottomLeft();
			Vector2 end = Bounds.BottomRight();

			for(int x = 0; x < numberOfRays; x++) // Down
			{
				stepPercent = (float)x / (float)(numberOfRays - 1);
				startPosition = Vector2.Lerp(origin, end, stepPercent);
				_listOfPoints.Add(startPosition);
				_listOfPoints.Add(startPosition - new Vector2(0, CollisionController.VerticalRayDistance));
			}

			numberOfRays = CollisionController.HoriztonalRayAmount;
			origin = Bounds.TopRight(); 
			end = Bounds.BottomRight();

			for(int x = 0; x < numberOfRays; x++) // Right
			{
				stepPercent = (float)x / (float)(numberOfRays - 1);
				startPosition = Vector2.Lerp(origin, end, stepPercent);
				_listOfPoints.Add(startPosition);
				_listOfPoints.Add(startPosition + new Vector2(CollisionController.HoriztonalRayDistance,0));
			}

			ReadOnlySpan<Vector3> pointsAsSpan = CollectionsMarshal.AsSpan(_listOfPoints);
			Gizmos.DrawLineList(pointsAsSpan);

            s_OnDrawGizmosMarker.End();
        }
#endif
	}
}