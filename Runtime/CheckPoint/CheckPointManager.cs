using SF.Events;

using UnityEngine;

namespace SF.SpawnModule
{
    public class CheckPointManager : MonoBehaviour, EventListener<CheckPointEvent>
    {
        public CheckPoint StartingCheckPoint;
        public CheckPoint CurrentCheckPoint;

        private void Start()
        {
			if(StartingCheckPoint != null && CurrentCheckPoint == null)
				CurrentCheckPoint = StartingCheckPoint;
        }
        public void OnEvent(CheckPointEvent checkPointEvent)
		{
			switch (checkPointEvent.EventType) 
			{
				case CheckPointEventTypes.ChangeCheckPoint:
					ChangeCheckPoint(checkPointEvent.CheckPoint);
					break;
			}
		}

		private void ChangeCheckPoint(CheckPoint checkPoint)
		{
			CurrentCheckPoint = checkPoint;
		}

		private void OnEnable()
		{
			this.EventStartListening<CheckPointEvent>();
		}
		private void OnDisable()
		{
			this.EventStopListening<CheckPointEvent>();
		}
	}
}
