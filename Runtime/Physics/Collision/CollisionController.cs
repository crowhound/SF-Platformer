using UnityEngine;

namespace SF.Physics.Collision
{
	[System.Serializable]
    public struct CollisionController
    {
		[Header("Ray distance")]
		public float HoriztonalRayDistance;
		public float VerticalRayDistance;

		[Header("Ray Amount")]
		public int HoriztonalRayAmount;
		public int VerticalRayAmount;

		[HideInInspector] public RaycastHit2D[] RaycastHit2Ds;
		public CollisionController(float horiztonalRayDistance = 0.01f,
							 float verticalRayDistance = 0.01f,
							 short horiztonalRayAmount = 3,
							 short verticalRayAmount = 3)
		{
			HoriztonalRayDistance = horiztonalRayDistance;
			VerticalRayDistance = verticalRayDistance;
			HoriztonalRayAmount = horiztonalRayAmount;
			VerticalRayAmount = verticalRayAmount;
			RaycastHit2Ds = new RaycastHit2D[4];
		}
	}
}
