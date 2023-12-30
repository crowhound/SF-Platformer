using SF.AbilityModule;
using UnityEngine;
namespace SF.Abilities.Characters
{
    public abstract class CharacterAbility : ICharacterAbility
    {
		public bool IsEnabled = true;

		[System.NonSerialized] protected GameObject _owner;
	}
}
