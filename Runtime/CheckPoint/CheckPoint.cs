using SF.SpawnModule;

using UnityEngine;

namespace SF
{
	public class CheckPoint : MonoBehaviour, ICheckPoint
	{
		public string CheckpointName;
		public LayerMask PlayerMask;
		private void OnTriggerEnter2D(Collider2D collision)
		{
			ActivateCheckPoint();
		}

		public void ActivateCheckPoint()
		{
			CheckPointEvent.Trigger(CheckPointEventTypes.ChangeCheckPoint,this);
		}
	}
}
