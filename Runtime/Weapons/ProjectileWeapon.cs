using System.Collections.Generic;
using UnityEngine;

using SF.ProjectileModule;


namespace SF.Weapons
{
    public class ProjectileWeapon : MonoBehaviour, IWeapon
    {
        [SerializeField] private bool _isAutoFire = false;
        [SerializeField] private Transform _spawnPosition;

        [SerializeField] private Projectile _projectile;
        [SerializeField] private Timer _cooldownTimer;
        [SerializeField] private float _fireCooldown;

        [SerializeField] private bool _isReady = false;
        private readonly List<GameObject> _pooledProjectiles = new();

        public void Start()
        {
            _cooldownTimer = new Timer(_fireCooldown, CooldownCompleted);

            if(_isAutoFire)
                Use();
        }

        public virtual void Use()
        {
            if(_projectile == null || _spawnPosition == null)
                return;

            _cooldownTimer.StartTimer();

            Projectile projectile = GameObject.Instantiate(_projectile);

            if(_isAutoFire)
                projectile.Fire(transform.position);
            else
                projectile.Fire(transform.position);
        }

        private void CooldownCompleted()
        {
            _isReady = true;
            _cooldownTimer.ResetTimer();
            Use();
        }
    }
}
