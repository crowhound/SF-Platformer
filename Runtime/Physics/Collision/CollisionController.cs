using UnityEngine;

namespace SF.Physics
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

		[field: Header("Ray Offsets")]
        [field: SerializeField] public float RayOffset { get; private set; }

        [HideInInspector] public RaycastHit2D[] RaycastHit2Ds;
		public CollisionController(float horiztonalRayDistance = 0.01f,
							 float verticalRayDistance = 0.01f,
							 short horiztonalRayAmount = 3,
							 short verticalRayAmount = 3,
							 float horiztonalOffset = 0.01f)
		{
			RayOffset = horiztonalOffset;
			HoriztonalRayDistance = horiztonalRayDistance;
			VerticalRayDistance = verticalRayDistance;
			HoriztonalRayAmount = horiztonalRayAmount;
			VerticalRayAmount = verticalRayAmount;
			RaycastHit2Ds = new RaycastHit2D[4];
		}
	}
}
