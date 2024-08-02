using SF.Characters.Controllers;

using UnityEngine;

namespace SF
{

    public class DirectionalForceManipulator : MonoBehaviour, IForceManipulator
    {
        public Vector2 Force;
        public ContactFilter2D ContactFilter;

        public void OnTriggerEnter2D(Collider2D other)
        {

            if((ContactFilter.layerMask & (1 << other.gameObject.layer)) == 0)
                return;

            if(other.TryGetComponent(out IForceReciever forceReciever) )
            {
                ExtertForce(forceReciever, Force);
            }
        }

        public void ExtertForce(IForceReciever forceReciever, Vector2 force)
        {
            forceReciever.SetExternalVelocity(force);
        }
    }
}