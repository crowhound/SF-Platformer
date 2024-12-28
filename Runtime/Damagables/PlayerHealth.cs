using SF.Events;

using UnityEngine;

namespace SF.SpawnModule
{
    public class PlayerHealth : CharacterHealth, IDamagable
    {
        protected override void Kill()
        {
            base.Kill();

            LivesEvent.Trigger(LivesEventTypes.DecreaseLives, 1);
            RespawnEvent.Trigger(RespawnEventTypes.PlayerRespawn);
            RespawnEvent.Trigger(RespawnEventTypes.GameObjectRespawn);
        }

        protected override void Respawn()
        {
            // TODO: Make the PlayerRespawn event pass in the check points position to
            // use it to move the player.
            if(CheckPointManager.Instance == null)
                return;

            if(CheckPointManager.Instance.CurrentCheckPoint != null)
                transform.position = CheckPointManager.Instance.CurrentCheckPoint.transform.position;

            base.Respawn();
        }
    }
}
