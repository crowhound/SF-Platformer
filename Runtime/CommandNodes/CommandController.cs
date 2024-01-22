using System.Collections.Generic;
using UnityEngine;

namespace SF.CommandModule
{
    public class CommandController : MonoBehaviour
    {
        [SerializeReference]
        public List<CommandNode> Commands = new List<CommandNode>();

        [SerializeField] private bool DoStart = false;

        public void Awake()
        {
        }

        public void Start()
        {
            if (DoStart)
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
