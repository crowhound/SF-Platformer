namespace SF
{
    public enum ScoreEventTypes
    {
        ScoreIncrease,
        ScoreDecrease,
        ScoreSet
    }
    public struct ScoreEvent
    {
        public ScoreEventTypes EventType;
        public float ScoreChange;
        public ScoreEvent(ScoreEventTypes eventType, float scoreChange = 0)
        {
            EventType = eventType;
            ScoreChange = scoreChange;
        }
        static ScoreEvent scoreEvent;

         public static void Trigger(ScoreEventTypes eventType, float scoreChange = 0)
         {
            scoreEvent.EventType = eventType;
            scoreEvent.ScoreChange = scoreChange;
         }
    }
}
