using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

using UnityEngine;

namespace SF.Interactables
{
	public enum InteractableMode
	{
		Collision,
		Input,
		Both // Think pressure plate that also needs a button to be switched on.
	}
	public interface IInteractable
	{
		InteractableMode InteractableMode { get; set; }
		void Interact();
	}
}