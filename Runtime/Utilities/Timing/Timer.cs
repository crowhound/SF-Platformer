using System;
using UnityEngine;

namespace SF
{
    [Serializable]
    public class Timer
    {
        public float StartingTime = 3;
        public float RemaingTime;

        /// <summary>
        /// An Action that is called at the end when the Timer hits 0.
        /// </summary>
        private Action _onTimerComplete;
        public Timer(Action onTimerComplete = null)
        {
            if (onTimerComplete == null)
             return;

            _onTimerComplete += onTimerComplete;
        }
        public Timer(float startingTime, Action onTimerComplete = null) : this(onTimerComplete)
        {
            StartingTime = startingTime;
            RemaingTime = startingTime;
        }
        public void ResetTimer()
        {
            RemaingTime = StartingTime;
        }

        public async Awaitable<bool> StartTimer()
        {
            ResetTimer();
            await UpdateTimer();
            return true;
        }

        public async Awaitable UpdateTimer()
        {
            while (RemaingTime > 0)
            {
                await Awaitable.EndOfFrameAsync();
                RemaingTime -= Time.deltaTime;
            }
            RemaingTime = 0;
            _onTimerComplete?.Invoke();
        }
    }
}
