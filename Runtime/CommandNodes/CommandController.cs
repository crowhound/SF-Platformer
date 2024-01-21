using System.Collections.Generic;
using UnityEngine;

namespace SF.CommandModule
{
    public class CommandController : MonoBehaviour
    {
        [SerializeReference]
        public List<CommandNode> Commands = new List<CommandNode>();

        public void Awake()
        {
        }

        public void OnValidate()
        {
            if (Commands.Count == 0)
            {
                Commands.Add(new TransformCommand(transform));
                Commands.Add(new SFXCommand());
                Commands.Add(new TransformCommand(transform));
            }
        }
        public void Start()
        {
            StartCommands();
        }

        public async Awaitable StartCommands()
        {
            for (int i = 0; i < Commands.Count; i++)
            {
                await Commands[i].Use();
            }
        }
    }
}
