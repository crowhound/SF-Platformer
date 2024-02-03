using UnityEngine;

namespace SF.CommandModule
{
    [System.Serializable]
    public abstract class CommandNode : ICommand
    {
        public string Name;
        public abstract Awaitable Use();
    }
}
