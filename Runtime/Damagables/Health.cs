using UnityEngine;
using SF.Events;

namespace SF.SpawnModule
{
    /// <summary>
    /// Adds a health system to anything. 
    /// This does not need to be on a character. You can add this to a crate or anything that wants to be damaged. There are checks in do stuff for character specific objects if you want to.
    /// </summary>
    public class Health : MonoBehaviour, IDamagable, EventListener<RespawnEvent>
    {
        public int CurrentHealth = 10;
        public int MaxHealth = 10;

        public virtual void TakeDamage(int damage)
        {
            if(!gameObject.activeSelf)
                return;

            CurrentHealth -= damage;

            if (CurrentHealth < 0)
                CurrentHealth = 0;
            if(CurrentHealth == 0)
                Kill();
        }

        public virtual void InstantKill()
        {
            CurrentHealth = 0;
            Kill();
		}

        protected virtual void Kill()
        {
            gameObject.SetActive(false);
        }

		public void OnEvent(RespawnEvent respawnEvent)
		{
			switch (respawnEvent.EventType) 
            {
                case RespawnEventTypes.PlayerRespawn:
                    Respawn();
                    break;
            }
		}

        protected virtual void Respawn()
        {
            CurrentHealth = MaxHealth;
            gameObject.SetActive(true);
        }

		protected void OnEnable()
		{
            this.EventStartListening<RespawnEvent>();
		}
		protected void OnDestroy()
        {
			this.EventStopListening<RespawnEvent>();
		}
    }
}