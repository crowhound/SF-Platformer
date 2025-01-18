using System.Collections.Generic;

using SF.Characters;
using SF.Characters.Controllers;
using SF.CombatModule;

using UnityEngine;

namespace SF.Weapons
{
    public abstract class WeaponBase : MonoBehaviour, IWeapon
    {
        [SerializeField] protected ComboType ComboType;
        public List<ComboAttack> ComboAttacks = new();
        [SerializeField] protected Vector2 _knockBackForce;
        [SerializeField] protected CharacterRenderer2D _character2D;
        [SerializeField] protected Controller2D _controller2D;
        [SerializeField] protected ContactFilter2D _hitBoxFilter;

        public virtual void Use()
        {

        }
    }
}
