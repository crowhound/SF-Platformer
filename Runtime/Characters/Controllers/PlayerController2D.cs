using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using SF.Utilities;

using UnityEngine;

namespace SF.Characters.Controllers
{
	public class PlayerController : GroundedController2D
    {


#if UNITY_EDITOR
		public void OnDrawGizmos()
		{

			_boxCollider = (_boxCollider == null) ? GetComponent<BoxCollider2D>() : _boxCollider;
			Bounds = _boxCollider.bounds;
			Vector2 startPosition;
			float stepPercent;
			int numberOfRays = CollisionController.VerticalRayAmount;
			Vector2 origin = Bounds.BottomLeft();
			Vector2 end = Bounds.BottomRight();
			List<Vector3> listOfPoints = new();

			for(int x = 0; x < numberOfRays; x++) // Down
			{
				stepPercent = (float)x / (float)(numberOfRays - 1);
				startPosition = Vector2.Lerp(origin, end, stepPercent);
				listOfPoints.Add(startPosition);
				listOfPoints.Add(startPosition - new Vector2(0, CollisionController.VerticalRayDistance));
			}

			numberOfRays = CollisionController.HoriztonalRayAmount;
			origin = Bounds.TopRight(); 
			end = Bounds.BottomRight();

			for(int x = 0; x < numberOfRays; x++) // Right
			{
				stepPercent = (float)x / (float)(numberOfRays - 1);
				startPosition = Vector2.Lerp(origin, end, stepPercent);
				listOfPoints.Add(startPosition);
				listOfPoints.Add(startPosition + new Vector2(CollisionController.HoriztonalRayDistance,0));
			}

			ReadOnlySpan<Vector3> pointsAsSpan = CollectionsMarshal.AsSpan(listOfPoints);
			Gizmos.DrawLineList(pointsAsSpan);
		}
#endif
	}
}