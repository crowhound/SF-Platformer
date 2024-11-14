using UnityEngine;

using SF.Characters;
using SF.Characters.Controllers;
using SF.CommandModule;
using SF.Events;
using SF.SpawnModule;


namespace SF
{
    /// <summary>
    /// Adds a health system to anything. 
    /// This does not need to be on a character. You can add this to a crate or anything that wants to be damaged. There are checks in do stuff for character specific objects if you want to.
    /// </summary>
    public class Health : MonoBehaviour, IDamagable, EventListener<RespawnEvent>
    {
        public CheckPointManager SpawnPoint;

        public CommandController CommandController;
        public int CurrentHealth = 10;
        public int MaxHealth = 10;

        [Header("Animation Setting")]

        [Tooltip("If you want to force an animation state when this object is damaged than set this string to the name of the animation state.")]
        public const string HitAnimationName = "Damaged";
        public readonly int HitAnimationHash = Animator.StringToHash(HitAnimationName);

        public const string DeathAnimationName = "Death";
        public readonly int DeathAnimationHash = Animator.StringToHash(DeathAnimationName);

        public float HitAnimationDuration = 0.3f;

        private Controller2D _controller;
        private Character2D _character2D;
        
        private void Awake()
        {
            _controller = GetComponent<Controller2D>();
            _character2D = GetComponent<Character2D>();

        }
        public virtual void TakeDamage(int damage)
        {
            if (CommandController != null)
                CommandController.StartCommands();

            if(_character2D != null && !string.IsNullOrEmpty(HitAnimationName))
                _character2D.SetAnimationState(HitAnimationName,HitAnimationDuration);

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
            if(_controller != null)
                _controller.CharacterState.CharacterStatus = CharacterStatus.Dead;

            if(_character2D != null && !string.IsNullOrEmpty(DeathAnimationName))
                _character2D.SetAnimationState(DeathAnimationName);
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

            if(_controller != null)
                _controller.Reset();

            if(SpawnPoint.CurrentCheckPoint != null)
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