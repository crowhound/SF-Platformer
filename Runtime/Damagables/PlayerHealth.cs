using UnityEngine;
using SF.Events;

namespace SF.SpawnModule
{
    public class PlayerHealth : CharacterHealth, IDamagable
    {
        [UnityEngine.SerializeField] private CheckPointManager SpawnPoint;

        protected override void Kill()
        {
            base.Kill();

            LivesEvent.Trigger(LivesEventTypes.DecreaseLives, 1);
            RespawnEvent.Trigger(RespawnEventTypes.PlayerRespawn);
            RespawnEvent.Trigger(RespawnEventTypes.GameObjectRespawn);
        }

        protected override void Respawn()
        {
            if(SpawnPoint == null)
                return;

            if(SpawnPoint.CurrentCheckPoint != null)
                transform.position = SpawnPoint.CurrentCheckPoint.transform.position;

            base.Respawn();
        }
    }
}
