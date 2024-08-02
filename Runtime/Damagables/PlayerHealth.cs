using UnityEngine;
using SF.Events;

namespace SF
{
    public class PlayerHealth : Health, IDamagable
    {
       
        protected override void Kill()
        {

            LivesEvent.Trigger(LivesEventTypes.DecreaseLives, 1);
            RespawnEvent.Trigger(RespawnEventTypes.PlayerRespawn);
            RespawnEvent.Trigger(RespawnEventTypes.GameObjectRespawn);
        }

        /*public void TakeDamage(int damage)
        {
            
        }*/
    }
}
