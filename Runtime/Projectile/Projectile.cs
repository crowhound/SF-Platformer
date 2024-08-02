using UnityEngine;

namespace SF.ProjectileModule
{
    [System.Serializable]
    public class Projectile : IProjectile
    {
        public float ProjectileSpeed = 5f;
        public float FireCooldown = 1f;
        [SerializeField] private bool _isReady = false;
        
        [SerializeField] private GameObject ProjectileObject;
        [SerializeField] private Timer _cooldownTimer;

        public void Init()
        {
            _cooldownTimer = new Timer(FireCooldown, CooldownCompleted);
        }
        public void Fire(Vector2 spawnPoint)
        {
            if (ProjectileObject == null || !_isReady)
                return;

            _isReady = false;
            _cooldownTimer.StartTimer();
            
            var projectile =  GameObject.Instantiate(ProjectileObject, spawnPoint, Quaternion.identity);

            projectile.GetComponent<Rigidbody2D>().linearVelocity = new Vector2(ProjectileSpeed, 0);
        }

        private void CooldownCompleted()
        {
            _isReady = true;
            _cooldownTimer.ResetTimer();
        }
    }
}
