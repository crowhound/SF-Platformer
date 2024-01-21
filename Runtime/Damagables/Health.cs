using UnityEngine;

namespace SF
{
    public class Health : MonoBehaviour, IDamagable
    {
        public int CurrentHealth = 10;
        public int MaxHealth = 10;

        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
                CurrentHealth = 0;
        }
    }
}