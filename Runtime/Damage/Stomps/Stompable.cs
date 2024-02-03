using UnityEngine;

namespace SF
{
    public class Stompable : MonoBehaviour, IStompable
    {
        private IDamagable _health;
        private void Awake()
        {
            _health = GetComponent<Health>();
        }
        public void Stomp()
        {
            if (_health == null)
                return;

            _health.TakeDamage(1);
        }
    }
}
