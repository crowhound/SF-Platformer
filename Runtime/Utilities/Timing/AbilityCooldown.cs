using UnityEngine;

namespace SF
{
    public class AbilityCooldown : MonoBehaviour
    {
        public Timer CoolDownTimer;
        public bool IsOnCooldown = false;
        public void Start()
        {
            CoolDownTimer = new Timer(3);
            StartCooldown();
        }

        private async void StartCooldown()
        {
            // First set it to false
            IsOnCooldown = false;
            // The CooldownTimer awaitable will return true when the timer finishes.
            IsOnCooldown = await CoolDownTimer.StartTimer();
        }
    }
}
