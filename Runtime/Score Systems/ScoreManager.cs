using UnityEngine;

using SF.Events;

namespace SF
{
    public class ScoreManager : MonoBehaviour, EventListener<ScoreEvent>
    {
        public float Score;
        public float LevelHighScore;
        public float ScoreMultiplier = 1;
        public void OnEvent(ScoreEvent scoreEvent)
        {
            switch (scoreEvent.EventType)
            {
                case ScoreEventTypes.ScoreIncrease:
                    ScoreChange(scoreEvent.ScoreChange);
                    break;
                case ScoreEventTypes.ScoreDecrease:
                    ScoreChange(scoreEvent.ScoreChange);
                    break;
                case ScoreEventTypes.ScoreSet:
                    ScoreChange(scoreEvent.ScoreChange, true);
                    break;
            }
        }
        private void ScoreChange(float scoreChange, bool settingScore = false)
        {
            Score = (settingScore)
                ? scoreChange
                : Score += scoreChange;
        }
        #region Enable/Disable
        private void OnEnable()
        {
             this.EventStartListening<ScoreEvent>();
        }
        private void OnDisable()
        {
            this.EventStopListening<ScoreEvent>();
        }
        #endregion
    }
}
