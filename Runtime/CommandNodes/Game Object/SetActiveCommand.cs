using System.Collections;
using System.Collections.Generic;
using SF.CommandModule;
using UnityEngine;

namespace SF
{
    public class SetActiveCommand : CommandNode, ICommand
    {
        [SerializeField] private bool _setActive = false;
        [SerializeField] private GameObject _gameObject;
        public SetActiveCommand() { }
        public SetActiveCommand( bool setActive = false)
        {
            _setActive = setActive;
        }
        public async override Awaitable Use()
        {
           _gameObject.SetActive(_setActive);
        }
    }
}
