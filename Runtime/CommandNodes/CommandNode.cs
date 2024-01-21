using UnityEngine;

namespace SF.CommandModule
{
    [System.Serializable]
    public abstract class CommandNode : ICommand
    {
        public string Name;
        public async virtual Awaitable Use() { }
    }
}
