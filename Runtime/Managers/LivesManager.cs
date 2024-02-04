using SF.Events;
using UnityEngine;

namespace SF
{
    [System.Serializable]
    public class LivesManager : EventListener<LivesEvent>
    {
        public int CurrentLives;
        public int MaxLives = 99;

        public void OnEvent(LivesEvent livesEvent)
        {
            switch(livesEvent.EventType)
            {
                case LivesEventTypes.IncreaseLives:
                    ChangeLives(livesEvent.Lives);
                    break;
                case LivesEventTypes.DecreaseLives:
                    ChangeLives(-livesEvent.Lives);
                    break;
            }
        }

        private void ChangeLives(int lives)
        {
            CurrentLives += lives;
        }

        public void RegisterEventListeners()
        {
            this.EventStartListening<LivesEvent>();
        }

        public void DeregisterEventListeners()
        {
            this.EventStopListening<LivesEvent>();
        }
    }
}
