using System.Collections.Generic;

using UnityEngine;

namespace SF.Weapons
{
    public class MeleeWeapon : WeaponBase, IWeapon
    {
        [SerializeField] private Collider2D _hitBox;
        private List<Collider2D> _hitResults = new();

        public override void Use()
        {
            if(_character2D != null)
                _character2D.SetAnimationState(ComboAttacks[0].Name);

            // TODO: Make the st
            _hitBox.Overlap(_hitBoxFilter, _hitResults);

            for(int i = 0; i < _hitResults.Count; i++)
            {
                if(_hitResults[i].TryGetComponent(out IDamagable damagable))
                {
                    damagable.TakeDamage(2);
                }
            }
        }
    }
}
