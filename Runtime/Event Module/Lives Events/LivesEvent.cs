namespace SF.Events
{
    public enum LivesEventTypes
    {
        IncreaseLives,
        DecreaseLives,
        SetLives
    }
    public struct LivesEvent
    {
        public LivesEventTypes EventType;
        public int Lives;
        public LivesEvent(LivesEventTypes eventType, int lives = 1)
        {
            EventType = eventType;
            Lives = lives;
        }
        static LivesEvent livesEvent;

         public static void Trigger(LivesEventTypes eventType, int lives )
         {
            livesEvent.EventType = eventType;
            livesEvent.Lives = lives;
            EventManager.TriggerEvent(livesEvent);
         }
    }
}
