using UnityEngine;

namespace SF
{
    public class Hazard : MonoBehaviour, IDamage
    {
        public int DamageAmount = 1;
        
        private void OnCollisionEnter2D(Collision2D collision2D)
        {
            if(collision2D.gameObject.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(DamageAmount);
            }
        }

        private void OnTriggerEnter2D(Collider2D collider2D)
        {
            if(collider2D.TryGetComponent(out IDamagable damagable))
            {
                damagable.TakeDamage(DamageAmount);
            }
        }
    }
}
