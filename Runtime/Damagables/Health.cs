using UnityEngine;
using SF.CommandModule;
using SF.Events;
using SF.SpawnModule;

namespace SF
{
    public class Health : MonoBehaviour, IDamagable, EventListener<RespawnEvent>
    {
        public CheckPointManager SpawnPoint;

        public CommandController CommandController;
        public int CurrentHealth = 10;
        public int MaxHealth = 10;

        public virtual void TakeDamage(int damage)
        {
            if (CommandController != null)
                CommandController.StartCommands();

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
            // This is more for things inheriting from Health.
            // Might have something in here later.
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

        protected void Respawn()
        {
            if(SpawnPoint == null)
                return;

            transform.position = SpawnPoint.CurrentCheckPoint.transform.position;
            CurrentHealth = MaxHealth;
        }

		protected void OnEnable()
		{
            this.EventStartListening<RespawnEvent>();
		}
		protected void OnDisable()
		{
			this.EventStopListening<RespawnEvent>();
		}
    }
}